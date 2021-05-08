namespace MicroService.Shared.ConsulServiceRegistration
{
    public interface IConsulServiceRegistry
    {
        /// <summary>
        /// 注销服务
        /// </summary>
        /// <param name="settings"></param>
        void Deregister(ConsulServiceOptions settings);
        
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="settings"></param>
        void Register(ConsulServiceOptions settings);
    }
}