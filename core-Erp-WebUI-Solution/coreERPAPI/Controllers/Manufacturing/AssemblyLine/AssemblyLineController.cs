//**********************************************//
//  	ASSEMBLY lINE - API CONTROLLER    		//
// 		 CREATOR: EMMANUEL OWUSU(MAN)			//
//			DATE: 6TH-10TH JULY					//
//**********************************************//

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using coreData.Constants;
using coreData.ErrorLog;

namespace coreErpApi.Controllers.Controllers.Manufacturing.AssemblyLine
{
    [AuthorizationFilter()]
    public class AssemblyLineController : ApiController
    {
        private ICommerceEntities le;
        private IcoreLoansEntities ctx;

        private string ErrorToReturn = "";

        ErrorMessages error = new ErrorMessages();

        public AssemblyLineController()
        {
            le = new CommerceEntities();
            ctx = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
        }

        public AssemblyLineController(ICommerceEntities lent, coreLoansEntities cent)
        {
            le = lent;
            ctx = cent;
        }

        // GET: api/assembly Line
        public IEnumerable<assemblyLine> Get()
        {
            return le.assemblyLines
                .Include(p => p.assemblyWorkStages)
                .OrderBy(p => p.assemblyLineName)
                .ToList();
        }

        // GET: api/assembly Line by Id
        [HttpGet]
        public assemblyLine Get(int id)
        {
            assemblyLine value = le.assemblyLines
                .Include(p => p.assemblyWorkStages)
                .FirstOrDefault(p => p.assemblyLineId == id);

            if (value == null)
            {
                value = new assemblyLine();
            }
            return value;
        }

        [HttpPost]
        public KendoResponse Get([FromBody]KendoRequest req)
        {
            string order = "assemblyLineName";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<depositType>(req, parameters);

            var query = le.assemblyLines.AsQueryable();
            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                query = query.Where(whereClause, parameters.ToArray());
            }

            var data = query
                .OrderBy(order.ToString())
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }

        [HttpPost]
        public assemblyLine Post(assemblyLine input)
        {
            if (!ValidateAssemblyLine(input))
            {
                Logger.logError(ErrorToReturn);
                throw new ApplicationException(ErrorToReturn);
            }

            input.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
            input.created = DateTime.Now;
            input.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            input.modified = DateTime.Now;

            foreach (var wrkStg in input.assemblyWorkStages)
            {
                if (!ValidateAssemblyWorkStage(wrkStg))
                {
                    Logger.logError(ErrorToReturn);
                    throw new ApplicationException(ErrorToReturn);
                }
                wrkStg.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                wrkStg.created = DateTime.Now;
                wrkStg.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
                wrkStg.modified = DateTime.Now;
            }

            le.assemblyLines
                .Add(input);

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.logError(ex);
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }

            return input;
        }


        [HttpPut]
        // PUT: api/assemblyLine
        public assemblyLine Put(assemblyLine input)
        {
            if (!ValidateAssemblyLine(input))
            {
                Logger.logError(ErrorToReturn);
                throw new ApplicationException(ErrorToReturn);
            }
            var toBeUpdated = le.assemblyLines.First(p => p.assemblyLineId == input.assemblyLineId);

            toBeUpdated.assemblyLineName = input.assemblyLineName;
            toBeUpdated.assemblyLineTypeId = input.assemblyLineTypeId;
            toBeUpdated.endProductId = input.endProductId;
            toBeUpdated.factoryId = input.factoryId;
            toBeUpdated.supervisorStaffId = input.supervisorStaffId;
            toBeUpdated.modifier = input.modifier;
            toBeUpdated.modified = input.modified;

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.logError(ex);
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }

            return toBeUpdated;
        }

        [HttpDelete]
        // DELETE: api/assemblyType
        public void Delete([FromBody]assemblyLine value)
        {
            var forDelete = le.assemblyLines.FirstOrDefault(p => p.assemblyLineId == value.assemblyLineId);
            if (forDelete != null)
            {
                le.assemblyLines.Remove(forDelete);
                try
                {
                    le.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.logError(ex);
                    throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
                }
            }
        }


        //Validate Fields in Assembly Line
        private bool ValidateAssemblyLine(assemblyLine assLin)
        {
            //If Shrinkage Grid is empty, Catch the error and return false
            if (assLin.assemblyWorkStages.Any())
            {
                //Execute the List List of Checks
                ValidateAssemblyLineDropDown(assLin);
                ValidateAssemblyLineNameField(assLin);

                //If errorMessage is empty test Pass
                if (ErrorToReturn == "")
                {
                    return true;
                }

                return false;
            }

            ErrorToReturn += error.GridWithoutDataErrorMessage;
            return false;
        }

        //Validate Fields in Assembly Line
        private bool ValidateAssemblyWorkStage(assemblyWorkStage assWkStage)
        {
            //Execute the List validation tests
            ValidateWorkStageDropDown(assWkStage);
            ValidateAssemblyWorkStageEmptyFields(assWkStage);
            ValidateAssemblyWorkStageFieldLength(assWkStage);

            //If errorMessage is empty test Pass
            if (ErrorToReturn == "")
            {
                return true;
            }

            return false;

        }

        private void ValidateAssemblyLineDropDown(assemblyLine assLin)
        {
            if (!assemblyLineTypeExist(assLin.assemblyLineTypeId) || !productExist(assLin.endProductId) ||
                !factoryExist(assLin.factoryId) || !supervisorExist(assLin.supervisorStaffId))
            {
                ErrorToReturn += error.AssemblyLineTypeError;
            }
        }

        private void ValidateWorkStageDropDown(assemblyWorkStage wrkStg)
        {
            if (!workStageTypeExist(wrkStg.workStageTypeId))
            {
                ErrorToReturn += error.AssemblyLineWorkStageError;
            }
        }

        private void ValidateAssemblyLineNameField(assemblyLine assLin)
        {
            if (string.IsNullOrWhiteSpace(assLin.assemblyLineName) || string.IsNullOrEmpty(assLin.assemblyLineName))
            {
                ErrorToReturn = error.AssemblyLineNameEmptyError;
            }
            if (assLin.assemblyLineName.Length > 29)
            {
                ErrorToReturn += error.AssemblyLineNameSizeError;
            }
        }

        private void ValidateAssemblyWorkStageEmptyFields(assemblyWorkStage assLinWkStg)
        {
            if (string.IsNullOrWhiteSpace(assLinWkStg.workStageName) || string.IsNullOrEmpty(assLinWkStg.workStageName)
                || string.IsNullOrWhiteSpace(assLinWkStg.workStageCode) || string.IsNullOrEmpty(assLinWkStg.workStageCode)
                || string.IsNullOrWhiteSpace(assLinWkStg.activityDescription) || string.IsNullOrEmpty(assLinWkStg.activityDescription))
            {
                ErrorToReturn = error.AssemblyLineEmptyWorkStageError;
            }
        }

        private void ValidateAssemblyWorkStageFieldLength(assemblyWorkStage assLinWkStg)
        {
            if (assLinWkStg.workStageName.Length > 20 || assLinWkStg.activityDescription.Length > 10
                || assLinWkStg.workStageCode.Length > 100)
            {
                ErrorToReturn += error.AssemblyLineEmptyWorkStageDataSize;
            }
        }




        private bool assemblyLineTypeExist(int typeId)
        {
            if (le.assemblyLineTypes.Any(p => p.assemblyLineTypeId == typeId))
            {
                return true;
            }

            return false;
        }

        private bool productExist(int prodId)
        {
            if (le.products.Any(p => p.productId == prodId))
            {
                return true;
            }

            return false;
        }

        private bool factoryExist(int factId)
        {
            if (le.factories.Any(p => p.factoryId == factId))
            {
                return true;
            }

            return false;
        }

        private bool supervisorExist(int staffId)
        {
            if (ctx.staffs.Any(p => p.staffID == staffId))
            {
                return true;
            }

            return false;
        }

        private bool workStageTypeExist(int wrkStgTypId)
        {
            if (le.workStageTypes.Any(p => p.workStageTypeId == wrkStgTypId))
            {
                return true;
            }

            return false;
        }



    }
}
