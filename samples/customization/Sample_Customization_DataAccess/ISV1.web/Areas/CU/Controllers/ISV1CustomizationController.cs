﻿using System;
using System.Web.Mvc;
using System.Linq;

using Microsoft.Web.Administration;
using Microsoft.Practices.Unity;
using System.Data.Entity;

using Sage.CA.SBS.ERP.Sage300.Common.Web;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Bootstrap;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Exceptions;

using ISV1.web.Areas.CU.Models;
using ISV1.web.Areas.CU.DAL.SageViews;
using ISV1.web.Areas.CU.DAL.SageViews.Repository;
using ISV1.web.Areas.CU.DAL.SageViews.Model;
using ISV1.web.Areas.CU.DAL.SageViews.ViewModel;

using ISV1.web.Areas.CU.DAL.CSQuery.Repository;
using ISV1.web.Areas.CU.DAL.CSQuery.Model;
using ISV1.web.Areas.CU.DAL.CSQuery.ViewModel;

using ISV1.web.Areas.CU.DAL.ADO_EF.Model;
using ISV1.web.Areas.CU.DAL.ADO_EF.Repository;
using ISV1.web.Areas.CU.DAL.ADO_EF.ViewModel;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Attributes;


namespace ISV1.web.Areas.CU.Controllers
{
    /// <summary>
    /// Controller needs to be registered in CUWebBootstrapper
    /// TODO: Add more custom controller actions
    /// </summary>
    public class ISV1CustomizationController<T> : MultitenantControllerBase<CustomerViewModel<T>>
        where T : Customer, new()
    {
        public ISV1CustomizationController()
        {
        }


        /// <summary>
        /// Default index view
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            ViewBag.Sage300Url = "http://localhost/Sage300";
            return View();
        }

        #region Action methods using Sage 300 View

        /// <summary>
        /// Get all entities keys by using Sage300c/Custom view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual JsonNetResult GetAllBySage300View()
        {
            SetUnityContainer(Context);

            var repository = new CustomerRepository<Customer>(Context);
            var modelData = repository.GetAll();

            return JsonNet(modelData);
        }

        /// <summary>
        /// Get entity by using Sage300c/Custom view
        /// </summary>
        /// <param name="id">customer number</param>
        /// <returns>Customer</returns>
        [HttpGet]
        public virtual JsonNetResult GetBySage300View(string id)
        {
            SetUnityContainer(Context);

            var repository = new CustomerRepository<Customer>(Context);
            var modelData = repository.GetById(id);

            // Set view model fields that required by UI
            var customerViewModel = new CustomerViewModel<T>();
            customerViewModel.Data = (T)modelData;
            customerViewModel.CurrencyCodeDescription = "Canadian Dollar";
            customerViewModel.IsMultiCurrency = true;
            customerViewModel.UserMessage = new UserMessage(modelData);
            
            return JsonNet(customerViewModel);
        }

        /// <summary>
        /// Save/Update entity by using Sage300c/Custom view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult SaveBySage300View(Customer model)
        {
            SetUnityContainer(Context);
            ViewModelBase<ModelBase> viewModel;

            if (!ValidateModelState(ModelState, out viewModel))
            {
                return JsonNet(viewModel);
            }
            var repository = new CustomerRepository<Customer>(Context);

            try
            {
                return JsonNet(repository.Save(model));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.SaveFailedMessage, businessException, "Customer"));
            }
        }

        /// <summary>
        /// Delete entity by using Sage300c/Custom view 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult DeleteBySage300View(string id)
        {
            SetUnityContainer(Context);

            var repository = new CustomerRepository<Customer>(Context);
            try
            {
                repository.Delete(id);
                return JsonNet("Delete successfully !");
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.DeleteFailedMessage, businessException, "Customer"));
            }
        }

        #endregion Call Sage 300 Controller Actions

        #region Action Methods using Sage 300 CSQuery

        /// <summary>
        /// Get Entity using Sage300 CS Query 
        /// </summary>
        /// <param name="id">entity id or key </param>
        /// <returns></returns>
        public virtual JsonNetResult GetBySage300CSQuery(string id)
        {
            SetUnityContainer(Context);

            var repository = new OrderRepository<Order>(Context);
            var modelData = repository.GetById(id);

            // Set view model fields that required by UI
            var customerViewModel = new OrderViewModel<Order>();
            customerViewModel.Data = (Order)modelData;
            customerViewModel.OrderCurrencyCode = "Canadian Dollar";
            customerViewModel.UserMessage = new UserMessage(modelData);

            return JsonNet(customerViewModel);
        }

        /// <summary>
        /// Get All Entity Keys using Sage300 CS Query 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual JsonNetResult GetAllBySage300CSQuery()
        {
            SetUnityContainer(Context);

            var repository = new OrderRepository<Order>(Context);
            var modelData = repository.GetAll();

            return JsonNet(modelData);
        }

        /// <summary>
        /// Save/Update Entity using Sage300 CS Query
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult SaveBySage300CSQuery(Order model)
        {
            SetUnityContainer(Context);

            ViewModelBase<ModelBase> viewModel;

            if (!ValidateModelState(ModelState, out viewModel))
            {
                return JsonNet(viewModel);
            }
            var repository = new OrderRepository<Order>(Context);

            try
            {
                return JsonNet(repository.Save(model));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.SaveFailedMessage, businessException, "Customer"));
            }
        }
        /// <summary>
        /// Delete entity using Sage300 CS Query
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult DeleteBySage300CSQuery(string id)
        {
            SetUnityContainer(Context);

            var repository = new OrderRepository<Order>(Context);
            try
            {
                repository.Delete(id);
                return JsonNet("Delete successfully !");
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.DeleteFailedMessage, businessException, "Customer"));
            }
        }

        #endregion 

        #region Action Methods using Custom View

        public virtual JsonNetResult GetByCustomView(string id)
        {
            SetUnityContainer(Context);

            var repository = new OrderRepository<Order>(Context);
            var modelData = repository.GetById(id);

            // Set view model fields
            var customerViewModel = new OrderViewModel<Order>();
            customerViewModel.Data = (Order)modelData;
            customerViewModel.OrderCurrencyCode = "Canadian Dollar";
            customerViewModel.UserMessage = new UserMessage(modelData);

            return JsonNet(customerViewModel);
        }

        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult SaveByCustomView(T model)
        {
            SetUnityContainer(Context);

            ViewModelBase<ModelBase> viewModel;

            if (!ValidateModelState(ModelState, out viewModel))
            {
                return JsonNet(viewModel);
            }
            var repository = new CustomerRepository<Customer>(Context);

            try
            {
                return JsonNet(repository.Save(model));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.SaveFailedMessage, businessException, "Customer"));
            }
        }

        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult DeleteByCustomView(string id)
        {
            SetUnityContainer(Context);

            var repository = new CustomerRepository<Customer>(Context);
            try
            {
                return JsonNet(repository.Delete(customer => customer.CustomerNumber == id));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.DeleteFailedMessage, businessException, "Customer"));
            }
        }

        #endregion 

        #region Action Methods Using Entity Framework to Acccess DB
        
        /// <summary>
        /// Get model data by id using Entity Framework direcly access SQL server DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual JsonNetResult GetByEntityFramework(string id)
        {
            //using generic reposity to get data
            var custRepository = new GenericRepository<ARCustomer>();
            var custData = custRepository.GetByID(id);
            var custOptRepository = new GenericRepository<ARCustomerOptionalField>();
            var custOptData = custOptRepository.GetListByFilter(e => e.CustomerNumber == id);
            custData.ARCustomerOptionalFields = custOptData.ToList();
            
            //using Entity Framework API to get Data
            using(var ctx = new CustomDbContext())
            {
                 var data = ctx.ARCustomer.Where(c => c.CustomerNumber == id).Include(e => e.ARCustomerOptionalFields);
            }
           

            // Set View Model Fields that UI needs
            var customerViewModel = new ARCustomerViewModel<ARCustomer>();
            customerViewModel.Data = (ARCustomer)custData;
            customerViewModel.CurrencyCodeDescription = "Canadian Dollar";

            return JsonNet(customerViewModel);
        }

        /// <summary>
        /// Get All Entities keys using Entity Framework direcly access SQL server DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual JsonNetResult GetAllByEntityFramework()
        {
            var repository = new GenericRepository<ARCustomer>();
            var modelData = repository.GetAll().Select(r => r.CustomerNumber.Trim());
            return JsonNet(modelData);
        }

        /// <summary>
        /// Add Entity using Entity Framework direcly access SQL server DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult AddByEntityFramework(ARCustomer model)
        {
            var repository = new GenericRepository<ARCustomer>();
            try
            {
                repository.Insert(model);
                return JsonNet("Added successfully !");
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.AddFailedMessage, businessException, "ARCustomer"));
            }
        }

        /// <summary>
        /// Save/Update Entity using Entity Framework direcly access SQL server DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult SaveByEntityFramework(ARCustomer model)
        {
            var repository = new GenericRepository<ARCustomer>();
            try
            {
                repository.Update(model);
                return JsonNet("Save successfully !");
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.SaveFailedMessage, businessException, "ARCustomer"));
            }
        }

        /// <summary>
        /// Delete Entity using Entity Framework direcly access SQL server DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult DeleteByEntityFramework(string id)
        {
            var repository = new GenericRepository<ARCustomer>();
            try
            {
                repository.Delete(id);
                return JsonNet("Delete successfully !");
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.DeleteFailedMessage, businessException, "Customer"));
            }
        }

        #endregion 

        /// <summary>
        /// Set unity container for integrated with sage 300c 
        /// </summary>
        /// <param name="Context"></param>
        private void SetUnityContainer(Context Context)
        {
            if (Context != null && Context.Container == null)
            {
                Context.Container = BootstrapTaskManager.Container;
            }
        }
    }

}