/*  
Copyright Microsoft Corporation

Licensed under the Apache License, Version 2.0 (the "License"); you may not
use this file except in compliance with the License. You may obtain a copy of
the License at 

http://www.apache.org/licenses/LICENSE-2.0 

THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED 
WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
MERCHANTABLITY OR NON-INFRINGEMENT. 

See the Apache 2 License for the specific language governing permissions and
limitations under the License. */

using System;
using System.Net;
using System.Web;
using MileageStats.Domain.Contracts;
using MileageStats.Domain.Contracts.Data;
using MileageStats.Domain.Properties;

namespace MileageStats.Domain.Handlers
{
    public class DeleteVehicle
    {
        private readonly IVehicleRepository _vehicleRepository;

        public DeleteVehicle(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public virtual void Execute(int userId, int vehicleId)
        {
            try
            {
                var vehicleToDelete = _vehicleRepository.GetVehicle(userId, vehicleId);

                if (vehicleToDelete != null)
                {
                    _vehicleRepository.Delete(vehicleId);
                }
                else
                {
                    throw new HttpException((int) HttpStatusCode.NotFound, Resources.VehicleNotFound);
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new UnauthorizedException(Resources.UnableToDeleteVehicleExceptionMessage, ex);
            }
        }
    }
}