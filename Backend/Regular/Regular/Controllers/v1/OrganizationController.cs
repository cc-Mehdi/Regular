using Azure.Core;
using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Regular.Controllers.v1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class OrganizationController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpContextAccessor _httpContextAccessor;
        
        public OrganizationController(IUnitOfWork unitOfWork, HttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("Organization")]
        public async Task<JsonResult> Organization(int id)
        {
            var Organization = _unitOfWork.OrganizationsRepository.GetFirstOrDefault(u => u.Id == id);
            var result = new OrganizationTemp()
            {
                PublicId = Organization.PublicId,
                Title = Organization.OrgTitle,
                OwnerId = Organization.OwnerId,
                Owner = Organization.Owner,
                ImageName = Organization.ImageName
            };
            return new JsonResult(result);
        }

        // get organizations by logged in user id
        [HttpGet("OrganizationByUserId")]
        public async Task<JsonResult> OrganizationByUserId(int id)
        {
            var OrganizationsList = _unitOfWork.OrganizationsRepository.GetAllByFilter(u => u.OwnerId == id).ToList();

            List<OrganizationTemp> result = new List<OrganizationTemp>();

            foreach (var item in OrganizationsList)
            {
                result.Add(new OrganizationTemp()
                {
                    PublicId = item.PublicId,
                    Title = item.OrgTitle,
                    OwnerId = item.OwnerId,
                    Owner = item.Owner,
                    ImageName = item.ImageName
                });
            }

            return new JsonResult(result);
        }

        // upsert organization
        public async Task<JsonResult> OnPostUpsertOrganizationAsync()
        {
            return new JsonResult(new { });
        //    var Helper_Models_User = new UsersController(_unitOfWork, webHostEnvironment);
        //    // Get logged-in user
        //    var loggedInUser = Helper_Models_User.LoggedInUser(_httpContext);

        //    if (!hasAccessToCreateOrganization())
        //        return new JsonResult(new { err = "شما به محدودیت ساخت سازمان رسیده اید!" });

        //    var title = _httpContext.Request.Form["OrgTitle"];
        //    var image = _httpContext.Request.Form.Files["ImageName"];
        //    var id = _httpContext.Request.Form["Id"];

        //    if (title == "")
        //        return new JsonResult(new { errorMessage = "لطفا یک عنوان برای سازمان انتخاب کنید" });

        //    // convert data to model for sending to database
        //    var newItem = new Organizations
        //    {
        //        OrgTitle = title,
        //        ImageName = "/CustomerResources/DefaultSources/OrgImage.png",
        //        OwnerId = loggedInUser.Id,
        //        Owner = loggedInUser,
        //        PublicId = Guid.NewGuid().ToString()
        //    };

        //    if (String.IsNullOrEmpty(id) || id == "0")
        //        _unitOfWork.OrganizationsRepository.Add(newItem);
        //    else
        //    {
        //        newItem.Id = int.Parse(id);
        //        _unitOfWork.OrganizationsRepository.Update(newItem);
        //    }

        //    _unitOfWork.Save();

        //    var OrganizationsList = _unitOfWork.OrganizationsRepository.GetAllByFilter(u => u.OwnerId == loggedInUser.Id).ToList();
        //    Organization = _unitOfWork.OrganizationsRepository.GetFirstOrDefault(u => u.Id == OrganizationsList[0].Id);
        //    return new JsonResult(OrganizationsList);
        }
    }

    // secure organization (without id field)
    internal class OrganizationTemp
    {
        public string PublicId { get; set; }
        public string Title { get; set; }
        public int OwnerId { get; set; }
        public Users Owner { get; set; }
        public string ImageName { get; set; }
    }
}
