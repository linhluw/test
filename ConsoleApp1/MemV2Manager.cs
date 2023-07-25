using System.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using MemV2Service.Service;

namespace SyncGPSMemory.ServiceImplementations
{
    public class MemV2ManagerService
    {
        private static ChannelFactory<ISyncSystemService> _factory;

        public static void Init()
        {
            CreateServices();
        }

        /// <summary>
        /// Tạo kết nối đến services
        /// </summary>
        private static bool CreateServices()
        {
            try
            {
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.MaxReceivedMessageSize = int.MaxValue;
                binding.MaxBufferPoolSize = int.MaxValue;
                binding.MaxBufferSize = int.MaxValue;
                binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
                binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
                binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
                binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
                var address = new EndpointAddress(GetEndpointAddress());
                _factory = new ChannelFactory<ISyncSystemService>(binding, address);

                // Client = new SyncSystemServiceClient(binding, new EndpointAddress(GetEndpointAddress()));

                return true;
            }
            catch (Exception)
            {
                //BA.Utilities.LogUtils.Exceptions.WriteLog(ex);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetEndpointAddress()
        {
            return "net.tcp://10.0.10.64:8085/SyncServices/";
            //return "net.tcp://10.1.11.104:8085/SyncServices/";
        }

        //public MemV2ManagerService(IOptions<MemoryOptions> memory)
        //{
        //    NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
        //    binding.MaxReceivedMessageSize = int.MaxValue;
        //    binding.MaxBufferPoolSize = int.MaxValue;
        //    binding.MaxBufferSize = int.MaxValue;
        //    binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
        //    binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
        //    binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
        //    binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
        //    var address = new EndpointAddress(memory.Value.Url);
        //    _factory = new ChannelFactory<ISyncSystemService>(binding, address);
        //}

        public static IEnumerable<VehicleExtend> GetInitVehicles(int XnCode, int CompanyID)
        {
            try
            {
                var channel = _factory.CreateChannel();
                var request = new VehilceOnlineRequest
                {
                    AccessToken = Guid.NewGuid().ToString(),
                    XNCode = XnCode,
                    CompanyID = CompanyID
                };
                var response = channel.InitOnlineVerhicle(request);
                return response?.Onlines?.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static IEnumerable<VehicleExtend> GetVehicle(int XnCode, string VehiclePlate)
        {
            try
            {
                var channel = _factory.CreateChannel();
                var request = new VehilceOnlineRequest
                {
                    AccessToken = Guid.NewGuid().ToString(),
                    XNCode = XnCode,
                    BKS = VehiclePlate
                };
                var response = channel.GetOneOnlineVerhicle(request);
                return response?.Onlines?.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static IEnumerable<VehicleExtend> GetInitOnlineVehicleListByCode(string json)
        {
            try
            {
                var channel = _factory.CreateChannel();
                var request = new VehilceOnlineRequest
                {
                    JsonVehicleInfoRequest = json
                };
                var response = channel.InitOnlineVehicleListByCode(request);
                var online = response?.Onlines?.ToList();
                Console.WriteLine("GetInitOnlineVehicleListByCode - OK | " + online.Count);
                return online;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Catch GetInitOnlineVehicleListByCode Invalid json object: {json}");
                return null;
            }

        }
    }
}
