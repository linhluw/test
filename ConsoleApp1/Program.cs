using BA_GPS_Server.VehicleOnlineGRPC;
using Grpc.Net.Client;
using MemV2Service.Service;
using Newtonsoft.Json;
using SyncGPSMemory.ServiceImplementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            MemV2ManagerService.Init();


            var obj = new List<VehicleInfoRequest>();

            //Catch GetInitOnlineVehicleListByCode Invalid json object: [{ "XNCode":45139,"VehiclePlate":"67A16682"}]
            obj.Add(new VehicleInfoRequest { XNCode = 45139, VehiclePlate = "67A16682" });


            var json = JsonConvert.SerializeObject(obj);

            while (true)
            {
                var memOnline = MemV2ManagerService.GetInitOnlineVehicleListByCode(json)?.ToList();
                if (memOnline != null)
                {
                    Console.WriteLine("Main - SUCCESS | " + memOnline.Count);
                }
                else
                {
                    Console.WriteLine("Main - FAIL");
                }
                Task.Delay(10000).Wait();
            }



        }
    }

    public class VehicleInfoRequest
    {
        public int XNCode { get; set; }

        public string VehiclePlate { get; set; }
    }
}
