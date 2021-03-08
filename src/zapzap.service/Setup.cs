namespace zapzap.service
{
    using Microsoft.Extensions.DependencyInjection;
    using zapzap.common.Providers;
    using zapzap.common.Repositories;
    using zapzap.common.Services;
    using zapzap.provider.websocket;
    using zapzap.repository;
    using zapzap.service.Handlers;
    using zapzap.service.Services;

    public static class Setup
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddSingleton<IWebSocketConnectionManager, WebSocketConnectionManager>();
            services.AddSingleton<IWebsocketHandler, WebSocketHandler>();
            services.AddSingleton<IGroupRepository, GroupRepository>();
            services.AddScoped<IGroupServices, GroupServices>();
            services.AddScoped<IChatHandler, ChatHandler>();

            return services;
        }
    }
}
