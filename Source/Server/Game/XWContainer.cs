using Microsoft.Extensions.DependencyInjection;
using Reoria.Engine.Container;

namespace Server;

public class XwContainer(IServiceCollection services) : EngineContainer(services)
{

}
