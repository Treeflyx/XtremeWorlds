// ===============================
// Client/Editor_Npc_Refactor.cs
// ===============================
//
// Major refactor goals achieved here:
// - All domain objects (NPC, Stats, Drops, etc.) are self‑contained and know how to (de)serialize themselves
//   to/from packets. No more global "Data.*" and "GameState.*" writes sprinkled in the UI.
// - A repository mediates persistence/network I/O. The UI talks to a ViewModel, not globals.
// - Safer, testable code: clear model boundaries, dirty tracking, and single-source‑of‑truth state.
//
// NOTE: This is a drop‑in architectural scaffold. Wire your real network client, asset paths,
//       and catalog sources where noted as TODOs.

#nullable enable
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Eto.Forms;
using Eto.Drawing;

// ===============================
// Infrastructure: Packets
// ===============================

public enum ClientOpcode : ushort
{
    NpcListRequest = 1000,
    NpcListResponse = 1001,
    NpcSave = 1010,
    NpcDelete = 1011
}

public sealed class Packet
{
    public ClientOpcode Opcode { get; }
    public byte[] Payload { get; }

    public Packet(ClientOpcode opcode, byte[] payload)
    {
        Opcode = opcode;
        Payload = payload;
    }
}

public interface IPacketSerializable
{
    void Serialize(PacketWriter w);
    void Deserialize(PacketReader r);
}

public sealed class PacketWriter : BinaryWriter
{
    public PacketWriter() : base(new MemoryStream(), Encoding.UTF8, leaveOpen: false) { }
    public byte[] ToArray() => ((MemoryStream)BaseStream).ToArray();

    // Helpers for variable‑length arrays / collections
    public void WriteList<T>(IReadOnlyList<T> list, Action<T> writeItem)
    {
        Write(list.Count);
        for (int i = 0; i < list.Count; i++) writeItem(list[i]);
    }
}

public sealed class PacketReader : BinaryReader
{
    public PacketReader(byte[] data) : base(new MemoryStream(data), Encoding.UTF8, leaveOpen: false) { }
    public List<T> ReadList<T>(Func<T> readItem)
    {
        int count = ReadInt32();
        var list = new List<T>(count);
        for (int i = 0; i < count; i++) list.Add(readItem());
        return list;
    }
}

// ===============================
// Domain: NPC & supporting types
// ===============================

public enum NpcBehaviour : byte { Stationary, Roam, Aggressive }
public enum NpcFaction : byte { Neutral, Friendly, Hostile }
public enum SpawnPeriod : byte { Any, Day, Night }

public sealed class StatBlock : IPacketSerializable, INotifyPropertyChanged
{
    public byte Strength  { get => _strength;  set => Set(ref _strength, value);  }
    public byte Intelligence { get => _intelligence; set => Set(ref _intelligence, value); }
    public byte Spirit    { get => _spirit;    set => Set(ref _spirit, value);    }
    public byte Luck      { get => _luck;      set => Set(ref _luck, value);      }
    public byte Vitality  { get => _vitality;  set => Set(ref _vitality, value);  }

    private byte _strength, _intelligence, _spirit, _luck, _vitality;

    public void Serialize(PacketWriter w)
    {
        w.Write(Strength);
        w.Write(Intelligence);
        w.Write(Spirit);
        w.Write(Luck);
        w.Write(Vitality);
    }

    public void Deserialize(PacketReader r)
    {
        Strength = r.ReadByte();
        Intelligence = r.ReadByte();
        Spirit = r.ReadByte();
        Luck = r.ReadByte();
        Vitality = r.ReadByte();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void Set<T>(ref T field, T val, [CallerMemberName] string? name = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, val))
        {
            field = val;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

public sealed class DropEntry : IPacketSerializable, INotifyPropertyChanged
{
    public int ItemId          { get => _itemId;          set => Set(ref _itemId, value); }
    public int Amount          { get => _amount;          set => Set(ref _amount, value); }
    public int ChancePercent   { get => _chancePercent;   set => Set(ref _chancePercent, value); }

    private int _itemId, _amount, _chancePercent;

    public void Serialize(PacketWriter w)
    {
        w.Write(ItemId);
        w.Write(Amount);
        w.Write(ChancePercent);
    }

    public void Deserialize(PacketReader r)
    {
        ItemId = r.ReadInt32();
        Amount = r.ReadInt32();
        ChancePercent = r.ReadInt32();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void Set<T>(ref T field, T val, [CallerMemberName] string? name = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, val))
        {
            field = val;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

public sealed class NpcModel : IPacketSerializable, INotifyPropertyChanged
{
    public int Id                { get => _id;                set => Set(ref _id, value); }
    public string Name           { get => _name;              set => Set(ref _name, value); }
    public string AttackSay      { get => _attackSay;         set => Set(ref _attackSay, value); }
    public int Sprite            { get => _sprite;            set => Set(ref _sprite, value); }
    public int SpawnSecs         { get => _spawnSecs;         set => Set(ref _spawnSecs, value); }
    public NpcBehaviour Behaviour{ get => _behaviour;         set => Set(ref _behaviour, value); }
    public NpcFaction Faction    { get => _faction;           set => Set(ref _faction, value); }
    public byte Range            { get => _range;             set => Set(ref _range, value); }
    public int Damage            { get => _damage;            set => Set(ref _damage, value); }
    public int AnimationId       { get => _animationId;       set => Set(ref _animationId, value); }
    public SpawnPeriod SpawnTime { get => _spawnTime;         set => Set(ref _spawnTime, value); }
    public int Hp                { get => _hp;                set => Set(ref _hp, value); }
    public int Exp               { get => _exp;               set => Set(ref _exp, value); }
    public byte Level            { get => _level;             set => Set(ref _level, value); }

    public StatBlock Stats { get; } = new();
    public int[] Skills { get; } = new int[6];
    public DropEntry[] Drops { get; } = Enumerable.Range(0, 6).Select(_ => new DropEntry()).ToArray();

    public bool IsDirty { get => _isDirty; private set => Set(ref _isDirty, value); }

    private int _id;
    private string _name = string.Empty;
    private string _attackSay = string.Empty;
    private int _sprite;
    private int _spawnSecs;
    private NpcBehaviour _behaviour;
    private NpcFaction _faction;
    private byte _range;
    private int _damage;
    private int _animationId;
    private SpawnPeriod _spawnTime;
    private int _hp;
    private int _exp;
    private byte _level;
    private bool _isDirty;

    public NpcModel()
    {
        // Bubble child changes to dirty flag
        Stats.PropertyChanged += (_, __) => MarkDirty();
        foreach (var d in Drops) d.PropertyChanged += (_, __) => MarkDirty();
    }

    public void MarkDirty() => IsDirty = true;
    public void ClearDirty() => IsDirty = false;

    public void Serialize(PacketWriter w)
    {
        w.Write(Id);
        w.Write(Name ?? string.Empty);
        w.Write(AttackSay ?? string.Empty);
        w.Write(Sprite);
        w.Write(SpawnSecs);
        w.Write((byte)Behaviour);
        w.Write((byte)Faction);
        w.Write(Range);
        w.Write(Damage);
        w.Write(AnimationId);
        w.Write((byte)SpawnTime);
        w.Write(Hp);
        w.Write(Exp);
        w.Write(Level);

        Stats.Serialize(w);

        // Skills
        w.Write(Skills.Length);
        for (int i = 0; i < Skills.Length; i++) w.Write(Skills[i]);

        // Drops
        w.Write(Drops.Length);
        for (int i = 0; i < Drops.Length; i++) Drops[i].Serialize(w);
    }

    public void Deserialize(PacketReader r)
    {
        Id = r.ReadInt32();
        Name = r.ReadString();
        AttackSay = r.ReadString();
        Sprite = r.ReadInt32();
        SpawnSecs = r.ReadInt32();
        Behaviour = (NpcBehaviour)r.ReadByte();
        Faction = (NpcFaction)r.ReadByte();
        Range = r.ReadByte();
        Damage = r.ReadInt32();
        AnimationId = r.ReadInt32();
        SpawnTime = (SpawnPeriod)r.ReadByte();
        Hp = r.ReadInt32();
        Exp = r.ReadInt32();
        Level = r.ReadByte();

        Stats.Deserialize(r);

        int skillCount = r.ReadInt32();
        for (int i = 0; i < Math.Min(skillCount, Skills.Length); i++) Skills[i] = r.ReadInt32();

        int dropCount = r.ReadInt32();
        for (int i = 0; i < Math.Min(dropCount, Drops.Length); i++) Drops[i].Deserialize(r);

        ClearDirty();
    }

    // Convenience helpers to build packets for save/delete
    public Packet BuildSavePacket()
    {
        var w = new PacketWriter();
        Serialize(w);
        return new Packet(ClientOpcode.NpcSave, w.ToArray());
    }

    public Packet BuildDeletePacket()
    {
        var w = new PacketWriter();
        w.Write(Id);
        return new Packet(ClientOpcode.NpcDelete, w.ToArray());
    }

    // Validation helper
    public void ValidateOrThrow()
    {
        if (string.IsNullOrWhiteSpace(Name)) throw new InvalidOperationException("Name is required.");
        if (Level > 255) throw new InvalidOperationException("Level out of bounds.");
        if (Drops.Any(d => d.ChancePercent < 0 || d.ChancePercent > 100))
            throw new InvalidOperationException("Drop chances must be 0..100.");
        // add more domain rules as needed
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void Set<T>(ref T field, T val, [CallerMemberName] string? name = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, val))
        {
            field = val;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            MarkDirty();
        }
    }

    public override string ToString() => $"{Id + 1}: {Name}";
}

// ===============================
// Catalogs / Lookups used by the editor (replace with your own data sources)
// ===============================
public sealed class GameCatalog
{
    public IReadOnlyList<string> Animations { get; init; } = Array.Empty<string>();
    public IReadOnlyList<string> Items       { get; init; } = Array.Empty<string>();
    public IReadOnlyList<string> Skills      { get; init; } = Array.Empty<string>();
}

// ===============================
// Networking
// ===============================
public interface INetworkClient
{
    Task SendAsync(Packet packet, CancellationToken ct = default);
    Task<Packet> RequestAsync(Packet packet, CancellationToken ct = default);
}

// A simple loopback/in-memory client for local development/demo (replace in production)
public sealed class LoopbackNetworkClient : INetworkClient
{
    private readonly List<NpcModel> _store;

    public LoopbackNetworkClient(IEnumerable<NpcModel>? seed = null)
    {
        _store = seed?.Select(Clone).ToList() ?? new List<NpcModel>();
    }

    public Task SendAsync(Packet packet, CancellationToken ct = default)
    {
        switch (packet.Opcode)
        {
            case ClientOpcode.NpcSave:
            {
                var r = new PacketReader(packet.Payload);
                var npc = new NpcModel();
                npc.Deserialize(r);

                var existing = _store.FirstOrDefault(x => x.Id == npc.Id);
                if (existing is null)
                    _store.Add(npc);
                else
                    _store[_store.IndexOf(existing)] = npc;
                break;
            }
            case ClientOpcode.NpcDelete:
            {
                var r = new PacketReader(packet.Payload);
                var id = r.ReadInt32();
                _store.RemoveAll(x => x.Id == id);
                break;
            }
        }
        return Task.CompletedTask;
    }

    public Task<Packet> RequestAsync(Packet packet, CancellationToken ct = default)
    {
        if (packet.Opcode == ClientOpcode.NpcListRequest)
        {
            var w = new PacketWriter();
            w.Write(_store.Count);
            foreach (var n in _store)
            {
                n.Serialize(w);
            }
            return Task.FromResult(new Packet(ClientOpcode.NpcListResponse, w.ToArray()));
        }
        throw new NotSupportedException($"Opcode {packet.Opcode} not supported by loopback client.");
    }

    private static NpcModel Clone(NpcModel n)
    {
        var w = new PacketWriter();
        n.Serialize(w);
        var r = new PacketReader(w.ToArray());
        var copy = new NpcModel();
        copy.Deserialize(r);
        return copy;
    }
}

// ===============================
// Repository (single source of truth for NPCs)
// ===============================
public sealed class NpcRepository
{
    private readonly INetworkClient _client;

    public ObservableCollection<NpcModel> Npcs { get; } = new();

    public GameCatalog Catalog { get; }

    public NpcRepository(INetworkClient client, GameCatalog catalog)
    {
        _client = client;
        Catalog = catalog;
    }

    public async Task LoadAsync(CancellationToken ct = default)
    {
        var req = new Packet(ClientOpcode.NpcListRequest, Array.Empty<byte>());
        var resp = await _client.RequestAsync(req, ct);
        if (resp.Opcode != ClientOpcode.NpcListResponse)
            throw new InvalidDataException("Unexpected response opcode.");

        var r = new PacketReader(resp.Payload);
        int count = r.ReadInt32();
        Npcs.Clear();
        for (int i = 0; i < count; i++)
        {
            var n = new NpcModel();
            n.Deserialize(r);
            Npcs.Add(n);
        }
    }

    public async Task SaveAsync(NpcModel npc, CancellationToken ct = default)
    {
        npc.ValidateOrThrow();
        await _client.SendAsync(npc.BuildSavePacket(), ct);
        npc.ClearDirty();
        // Optionally re‑load list or update local cache as needed
    }

    public Task DeleteAsync(NpcModel npc, CancellationToken ct = default)
        => _client.SendAsync(npc.BuildDeletePacket(), ct);
}

// ===============================
// ViewModel for the editor
// ===============================
public sealed class NpcEditorViewModel : INotifyPropertyChanged
{
    private readonly NpcRepository _repo;

    public ObservableCollection<NpcModel> Npcs => _repo.Npcs;

    private NpcModel? _selectedNpc;
    public NpcModel? SelectedNpc
    {
        get => _selectedNpc;
        set { if (_selectedNpc != value) { _selectedNpc = value; OnPropertyChanged(); } }
    }

    public GameCatalog Catalog => _repo.Catalog;

    public NpcEditorViewModel(NpcRepository repo) => _repo = repo;

    public Task LoadAsync(CancellationToken ct = default) => _repo.LoadAsync(ct);
    public Task SaveSelectedAsync(CancellationToken ct = default)
        => _selectedNpc is null ? Task.CompletedTask : _repo.SaveAsync(_selectedNpc, ct);
    public async Task DeleteSelectedAsync(CancellationToken ct = default)
    {
        if (_selectedNpc is null) return;
        var toDelete = _selectedNpc;
        await _repo.DeleteAsync(toDelete, ct);
        _ = _repo.Npcs.Remove(toDelete);
        SelectedNpc = _repo.Npcs.FirstOrDefault();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

// ===============================
// Asset provider (sprite path lookup)
// ===============================
public interface IAssetProvider
{
    string? TryGetCharacterSpritePath(int spriteId);
}

// Replace with your real data path logic
public sealed class DefaultAssetProvider : IAssetProvider
{
    public string? TryGetCharacterSpritePath(int spriteId)
    {
        // TODO: Plug in your path logic here
        var ext = ".png"; // or ".bmp" if that's your pipeline
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "characters", $"{spriteId}{ext}");
        return File.Exists(path) ? path : null;
    }
}

// ===============================
// UI: Eto.Forms Editor_Npc (refactored to ViewModel)
// ===============================
public sealed class Editor_Npc : Form
{
    // --- UI elements ---
    private readonly ListBox lstIndex = new() { Size = new Size(220, -1) };
    private readonly TextBox txtName = new();
    private readonly TextBox txtAttackSay = new();
    private readonly NumericStepper nudSprite = new() { MinValue = 0, MaxValue = 9999, DecimalPlaces = 0, Width = 80 };
    private readonly NumericStepper nudSpawnSecs = new() { MinValue = 0, MaxValue = 3600, DecimalPlaces = 0, Width = 80 };
    private readonly ComboBox cmbBehaviour = new();
    private readonly ComboBox cmbFaction = new();
    private readonly NumericStepper nudRange = new() { MinValue = 0, MaxValue = 50, DecimalPlaces = 0, Width = 80 };
    private readonly NumericStepper nudDamage = new() { MinValue = 0, MaxValue = 1000000, DecimalPlaces = 0, Width = 100 };
    private readonly ComboBox cmbAnimation = new();
    private readonly ComboBox cmbSpawnPeriod = new();

    private readonly NumericStepper nudHp = new() { MinValue = 0, MaxValue = 10_000_000, DecimalPlaces = 0, Width = 100 };
    private readonly NumericStepper nudExp = new() { MinValue = 0, MaxValue = 10_000_000, DecimalPlaces = 0, Width = 100 };
    private readonly NumericStepper nudLevel = new() { MinValue = 0, MaxValue = 255, DecimalPlaces = 0, Width = 80 };

    private readonly NumericStepper nudStrength = new() { MinValue = 0, MaxValue = 255, DecimalPlaces = 0, Width = 70 };
    private readonly NumericStepper nudIntelligence = new() { MinValue = 0, MaxValue = 255, DecimalPlaces = 0, Width = 70 };
    private readonly NumericStepper nudSpirit = new() { MinValue = 0, MaxValue = 255, DecimalPlaces = 0, Width = 70 };
    private readonly NumericStepper nudLuck = new() { MinValue = 0, MaxValue = 255, DecimalPlaces = 0, Width = 70 };
    private readonly NumericStepper nudVitality = new() { MinValue = 0, MaxValue = 255, DecimalPlaces = 0, Width = 70 };

    private readonly ComboBox cmbSkill1 = new();
    private readonly ComboBox cmbSkill2 = new();
    private readonly ComboBox cmbSkill3 = new();
    private readonly ComboBox cmbSkill4 = new();
    private readonly ComboBox cmbSkill5 = new();
    private readonly ComboBox cmbSkill6 = new();

    private readonly ComboBox cmbDropSlot = new();
    private readonly ComboBox cmbItem = new();
    private readonly NumericStepper nudAmount = new() { MinValue = 0, MaxValue = 1_000_000, DecimalPlaces = 0, Width = 100 };
    private readonly NumericStepper nudChance = new() { MinValue = 0, MaxValue = 100, DecimalPlaces = 0, Width = 80 };

    private readonly Drawable picSprite = new() { Size = new Size(96, 96), BackgroundColor = Colors.Black };

    private readonly Button btnSave = new() { Text = "Save" };
    private readonly Button btnCancel = new() { Text = "Close" };
    private readonly Button btnDelete = new() { Text = "Delete" };
    private Bitmap? _spriteBitmap;
    private readonly IAssetProvider _assets;

    // VM & flags
    private readonly NpcEditorViewModel _vm;
    private bool _init;

    public Editor_Npc()
    {
        Title = "NPC Editor (Refactored)";
        ClientSize = new Size(1100, 640);
        MinimumSize = new Size(1000, 600);

        // === Build Repo + VM ===
        // TODO: replace LoopbackNetworkClient + demo seed with your real client & data
        var demoSeed = new[]
        {
            new NpcModel { Id=0, Name="Slime", Level=1, Hp=12, Sprite=1, Behaviour=NpcBehaviour.Roam, Faction=NpcFaction.Neutral, Damage=2, Range=1, AnimationId=0, Exp=5, SpawnSecs=5, SpawnTime=SpawnPeriod.Any },
            new NpcModel { Id=1, Name="Skeleton", Level=3, Hp=40, Sprite=2, Behaviour=NpcBehaviour.Aggressive, Faction=NpcFaction.Hostile, Damage=8, Range=1, AnimationId=1, Exp=15, SpawnSecs=10, SpawnTime=SpawnPeriod.Night }
        };

        var catalog = new GameCatalog
        {
            Animations = new[] { "Idle", "Walk", "Attack", "Die" },
            Items      = Enumerable.Range(1, 20).Select(i => $"Item {i}").ToArray(),
            Skills     = Enumerable.Range(1, 12).Select(i => $"Skill {i}").ToArray()
        };

        var repo = new NpcRepository(new LoopbackNetworkClient(demoSeed), catalog);
        _vm = new NpcEditorViewModel(repo);
        _assets = new DefaultAssetProvider();

        InitializeUi();
        Shown += async (_, __) =>
        {
            await _vm.LoadAsync();
            PopulateList();
            lstIndex.SelectedIndex = _vm.Npcs.Count > 0 ? 0 : -1;
        };
    }

    private void InitializeUi()
    {
        // LEFT: NPC list
        lstIndex.SelectedIndexChanged += (_, __) =>
        {
            if (_init) return;
            var idx = lstIndex.SelectedIndex;
            _vm.SelectedNpc = (idx >= 0 && idx < _vm.Npcs.Count) ? _vm.Npcs[idx] : null;
            LoadSelectedIntoUi();
        };

        // General combos
        cmbBehaviour.Items.AddRange(Enum.GetNames(typeof(NpcBehaviour)));
        cmbFaction.Items.AddRange(Enum.GetNames(typeof(NpcFaction)));
        cmbSpawnPeriod.Items.AddRange(Enum.GetNames(typeof(SpawnPeriod)));

        // Catalog combos
        cmbAnimation.Items.AddRange(_vm.Catalog.Animations);
        foreach (var cmb in new[] { cmbSkill1, cmbSkill2, cmbSkill3, cmbSkill4, cmbSkill5, cmbSkill6 })
            cmb.Items.AddRange(_vm.Catalog.Skills);
        cmbItem.Items.AddRange(_vm.Catalog.Items);

        // Drops: 6 slots
        for (int i = 0; i < 6; i++) cmbDropSlot.Items.Add((i + 1).ToString());
        cmbDropSlot.SelectedIndex = 0;
        cmbDropSlot.SelectedIndexChanged += (_, __) => SyncDropFields(readFromNpc: true);

        // Field events -> VM
        txtName.TextChanged += (_, __) => SetNpc(n => n.Name = txtName.Text.Trim());
        txtAttackSay.TextChanged += (_, __) => SetNpc(n => n.AttackSay = txtAttackSay.Text);
        nudSprite.ValueChanged += (_, __) => { SetNpc(n => n.Sprite = (int)nudSprite.Value); DrawSprite(); };
        nudSpawnSecs.ValueChanged += (_, __) => SetNpc(n => n.SpawnSecs = (int)nudSpawnSecs.Value);
        cmbBehaviour.SelectedIndexChanged += (_, __) => SetNpc(n => n.Behaviour = (NpcBehaviour)cmbBehaviour.SelectedIndex);
        cmbFaction.SelectedIndexChanged += (_, __) => SetNpc(n => n.Faction = (NpcFaction)cmbFaction.SelectedIndex);
        nudRange.ValueChanged += (_, __) => SetNpc(n => n.Range = (byte)nudRange.Value);
        nudDamage.ValueChanged += (_, __) => SetNpc(n => n.Damage = (int)nudDamage.Value);
        cmbAnimation.SelectedIndexChanged += (_, __) => SetNpc(n => n.AnimationId = cmbAnimation.SelectedIndex);
        cmbSpawnPeriod.SelectedIndexChanged += (_, __) => SetNpc(n => n.SpawnTime = (SpawnPeriod)cmbSpawnPeriod.SelectedIndex);

        nudHp.ValueChanged += (_, __) => SetNpc(n => n.Hp = (int)nudHp.Value);
        nudExp.ValueChanged += (_, __) => SetNpc(n => n.Exp = (int)nudExp.Value);
        nudLevel.ValueChanged += (_, __) => SetNpc(n => n.Level = (byte)nudLevel.Value);

        nudStrength.ValueChanged += (_, __) => SetNpc(n => n.Stats.Strength = (byte)nudStrength.Value);
        nudIntelligence.ValueChanged += (_, __) => SetNpc(n => n.Stats.Intelligence = (byte)nudIntelligence.Value);
        nudSpirit.ValueChanged += (_, __) => SetNpc(n => n.Stats.Spirit = (byte)nudSpirit.Value);
        nudLuck.ValueChanged += (_, __) => SetNpc(n => n.Stats.Luck = (byte)nudLuck.Value);
        nudVitality.ValueChanged += (_, __) => SetNpc(n => n.Stats.Vitality = (byte)nudVitality.Value);

        cmbSkill1.SelectedIndexChanged += (_, __) => SetNpc(n => n.Skills[0] = cmbSkill1.SelectedIndex);
        cmbSkill2.SelectedIndexChanged += (_, __) => SetNpc(n => n.Skills[1] = cmbSkill2.SelectedIndex);
        cmbSkill3.SelectedIndexChanged += (_, __) => SetNpc(n => n.Skills[2] = cmbSkill3.SelectedIndex);
        cmbSkill4.SelectedIndexChanged += (_, __) => SetNpc(n => n.Skills[3] = cmbSkill4.SelectedIndex);
        cmbSkill5.SelectedIndexChanged += (_, __) => SetNpc(n => n.Skills[4] = cmbSkill5.SelectedIndex);
        cmbSkill6.SelectedIndexChanged += (_, __) => SetNpc(n => n.Skills[5] = cmbSkill6.SelectedIndex);

        cmbItem.SelectedIndexChanged += (_, __) => SetDrop(d => d.ItemId = cmbItem.SelectedIndex);
        nudAmount.ValueChanged += (_, __) => SetDrop(d => d.Amount = (int)nudAmount.Value);
        nudChance.ValueChanged += (_, __) => SetDrop(d => d.ChancePercent = (int)nudChance.Value);

        // Sprite preview
        picSprite.Paint += (_, e) =>
        {
            if (_spriteBitmap != null)
            {
                // If your spritesheet has 4x4 frames, display first frame
                int frameW = _spriteBitmap.Width / 4;
                int frameH = _spriteBitmap.Height / 4;
                e.Graphics.DrawImage(_spriteBitmap, new Rectangle(0, 0, frameW, frameH));
            }
        };

        // Buttons
        btnSave.Click += async (_, __) =>
        {
            if (_vm.SelectedNpc is null) return;
            try
            {
                await _vm.SaveSelectedAsync();
                MessageBox.Show(this, "NPC saved.", MessageBoxType.Information);
                RefreshList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Save failed: {ex.Message}", MessageBoxType.Error);
            }
        };
        btnDelete.Click += async (_, __) =>
        {
            if (_vm.SelectedNpc is null) return;
            if (MessageBox.Show(this, $"Delete '{_vm.SelectedNpc.Name}'?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                await _vm.DeleteSelectedAsync();
                PopulateList();
            }
        };
        btnCancel.Click += (_, __) => Close();

        // Layout
        var generalGroup = new GroupBox { Text = "General", Content = new TableLayout
        {
            Spacing = new Size(4,4),
            Rows =
            {
                TR("Name:", txtName),
                TR("Attack Say:", txtAttackSay),
                new TableRow(new Label{Text="Sprite:"}, new StackLayout { Orientation=Orientation.Horizontal, Items={ nudSprite, picSprite } }),
                TR("Animation:", cmbAnimation),
                TR("Spawn Secs:", nudSpawnSecs),
                TR("Spawn Period:", cmbSpawnPeriod),
                TR("Behaviour:", cmbBehaviour),
                TR("Faction:", cmbFaction),
                TR("Range:", nudRange),
                TR("Damage:", nudDamage),
            }
        }};

        var statsGroup = new GroupBox { Text = "Stats", Content = new TableLayout
        {
            Spacing = new Size(4,4),
            Rows =
            {
                TR("HP:", nudHp),
                TR("EXP:", nudExp),
                TR("Level:", nudLevel),
                TR("Strength:", nudStrength),
                TR("Intelligence:", nudIntelligence),
                TR("Spirit:", nudSpirit),
                TR("Luck:", nudLuck),
                TR("Vitality:", nudVitality),
            }
        }};

        var skillsGroup = new GroupBox { Text = "Skills", Content = new TableLayout
        {
            Spacing = new Size(4,4),
            Rows =
            {
                TR("Skill 1:", cmbSkill1),
                TR("Skill 2:", cmbSkill2),
                TR("Skill 3:", cmbSkill3),
                TR("Skill 4:", cmbSkill4),
                TR("Skill 5:", cmbSkill5),
                TR("Skill 6:", cmbSkill6),
            }
        }};

        var dropsGroup = new GroupBox { Text = "Drops", Content = new TableLayout
        {
            Spacing = new Size(4,4),
            Rows =
            {
                TR("Slot:", cmbDropSlot),
                TR("Item:", cmbItem),
                TR("Amount:", nudAmount),
                TR("Chance %:", nudChance),
            }
        }};

        var rightPanel = new Scrollable
        {
            Content = new StackLayout
            {
                Padding = 6,
                Spacing = 10,
                Items =
                {
                    generalGroup,
                    statsGroup,
                    skillsGroup,
                    dropsGroup,
                    new StackLayout { Orientation = Orientation.Horizontal, Spacing = 6, Items = { btnSave, btnDelete, btnCancel } }
                }
            }
        };

        Content = new Splitter
        {
            Position = 260,
            Panel1 = new StackLayout
            {
                Padding = 8,
                Spacing = 6,
                Items =
                {
                    new Label{ Text="NPCs", Font = SystemFonts.Bold(12) },
                    lstIndex
                }
            },
            Panel2 = rightPanel
        };
    }

    private static TableRow TR(string label, Control control) => new(new Label { Text = label }, control);

    private void PopulateList()
    {
        lstIndex.Items.Clear();
        foreach (var n in _vm.Npcs)
            lstIndex.Items.Add(new ListItem { Text = n.ToString() });
    }

    private void RefreshList()
    {
        // Update list text for items (id/name)
        for (int i = 0; i < _vm.Npcs.Count; i++)
        {
            if (lstIndex.Items[i] is ListItem li)
                li.Text = _vm.Npcs[i].ToString();
        }
        lstIndex.Invalidate();
    }

    private void LoadSelectedIntoUi()
    {
        _init = true;
        try
        {
            var n = _vm.SelectedNpc;
            bool has = n is not null;

            txtName.Enabled = has;
            txtAttackSay.Enabled = has;
            nudSprite.Enabled = has;
            nudSpawnSecs.Enabled = has;
            cmbBehaviour.Enabled = has;
            cmbFaction.Enabled = has;
            nudRange.Enabled = has;
            nudDamage.Enabled = has;
            cmbAnimation.Enabled = has;
            cmbSpawnPeriod.Enabled = has;
            nudHp.Enabled = has;
            nudExp.Enabled = has;
            nudLevel.Enabled = has;
            nudStrength.Enabled = has;
            nudIntelligence.Enabled = has;
            nudSpirit.Enabled = has;
            nudLuck.Enabled = has;
            nudVitality.Enabled = has;
            cmbSkill1.Enabled = cmbSkill2.Enabled = cmbSkill3.Enabled =
                cmbSkill4.Enabled = cmbSkill5.Enabled = cmbSkill6.Enabled = has;
            cmbDropSlot.Enabled = cmbItem.Enabled = nudAmount.Enabled = nudChance.Enabled = has;

            if (!has)
            {
                ClearFields();
                _spriteBitmap = null;
                picSprite.Invalidate();
                return;
            }

            txtName.Text = n!.Name;
            txtAttackSay.Text = n.AttackSay;
            nudSprite.Value = n.Sprite;
            nudSpawnSecs.Value = n.SpawnSecs;
            cmbBehaviour.SelectedIndex = (int)n.Behaviour;
            cmbFaction.SelectedIndex = (int)n.Faction;
            nudRange.Value = n.Range;
            nudDamage.Value = n.Damage;
            cmbAnimation.SelectedIndex = n.AnimationId;
            cmbSpawnPeriod.SelectedIndex = (int)n.SpawnTime;

            nudHp.Value = n.Hp;
            nudExp.Value = n.Exp;
            nudLevel.Value = n.Level;

            nudStrength.Value = n.Stats.Strength;
            nudIntelligence.Value = n.Stats.Intelligence;
            nudSpirit.Value = n.Stats.Spirit;
            nudLuck.Value = n.Stats.Luck;
            nudVitality.Value = n.Stats.Vitality;

            var skills = n.Skills;
            cmbSkill1.SelectedIndex = SafeIdx(skills, 0);
            cmbSkill2.SelectedIndex = SafeIdx(skills, 1);
            cmbSkill3.SelectedIndex = SafeIdx(skills, 2);
            cmbSkill4.SelectedIndex = SafeIdx(skills, 3);
            cmbSkill5.SelectedIndex = SafeIdx(skills, 4);
            cmbSkill6.SelectedIndex = SafeIdx(skills, 5);

            SyncDropFields(readFromNpc: true);
            DrawSprite();
        }
        finally { _init = false; }
    }

    private static int SafeIdx(int[] array, int idx)
        => (idx >= 0 && idx < array.Length) ? array[idx] : -1;

    private void ClearFields()
    {
        txtName.Text = string.Empty;
        txtAttackSay.Text = string.Empty;
        nudSprite.Value = 0;
        nudSpawnSecs.Value = 0;
        cmbBehaviour.SelectedIndex = -1;
        cmbFaction.SelectedIndex = -1;
        nudRange.Value = 0;
        nudDamage.Value = 0;
        cmbAnimation.SelectedIndex = -1;
        cmbSpawnPeriod.SelectedIndex = -1;

        nudHp.Value = 0;
        nudExp.Value = 0;
        nudLevel.Value = 0;

        nudStrength.Value = 0;
        nudIntelligence.Value = 0;
        nudSpirit.Value = 0;
        nudLuck.Value = 0;
        nudVitality.Value = 0;

        cmbSkill1.SelectedIndex = -1;
        cmbSkill2.SelectedIndex = -1;
        cmbSkill3.SelectedIndex = -1;
        cmbSkill4.SelectedIndex = -1;
        cmbSkill5.SelectedIndex = -1;
        cmbSkill6.SelectedIndex = -1;

        cmbDropSlot.SelectedIndex = 0;
        cmbItem.SelectedIndex = -1;
        nudAmount.Value = 0;
        nudChance.Value = 0;
    }

    private void SetNpc(Action<NpcModel> set)
    {
        if (_init || _vm.SelectedNpc is null) return;
        set(_vm.SelectedNpc);
        RefreshList(); // keep list names in sync
    }

    private void SetDrop(Action<DropEntry> set)
    {
        if (_init || _vm.SelectedNpc is null) return;
        var slot = Math.Clamp(cmbDropSlot.SelectedIndex, 0, 5);
        var drop = _vm.SelectedNpc.Drops[slot];
        set(drop);
    }

    private void SyncDropFields(bool readFromNpc)
    {
        if (_vm.SelectedNpc is null || cmbDropSlot.SelectedIndex < 0) return;

        _init = true;
        try
        {
            var slot = Math.Clamp(cmbDropSlot.SelectedIndex, 0, 5);
            var d = _vm.SelectedNpc.Drops[slot];
            if (readFromNpc)
            {
                cmbItem.SelectedIndex = d.ItemId;
                nudAmount.Value = d.Amount;
                nudChance.Value = d.ChancePercent;
            }
            else
            {
                d.ItemId = cmbItem.SelectedIndex;
                d.Amount = (int)nudAmount.Value;
                d.ChancePercent = (int)nudChance.Value;
            }
        }
        finally { _init = false; }
    }

    private void DrawSprite()
    {
        _spriteBitmap = null;
        picSprite.Invalidate();
        if (_vm.SelectedNpc is null) return;

        var id = _vm.SelectedNpc.Sprite;
        var path = _assets.TryGetCharacterSpritePath(id);
        if (path is null) return;

        try
        {
            using var fs = File.OpenRead(path);
            _spriteBitmap = new Bitmap(fs);
        }
        catch { _spriteBitmap = null; }
        picSprite.Invalidate();
    }
}
