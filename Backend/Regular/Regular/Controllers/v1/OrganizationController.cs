using Azure.Core;
using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Regular.Controllers.v1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class OrganizationController
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public OrganizationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<JsonResult> Organization(int id)
        {
            var Organization = _unitOfWork.OrganizationsRepository.GetFirstOrDefault(u => u.Id == id);
            var result = new OrganizationTemp()
            {
                PublicId = Organization.PublicId,
                Title = Organization.Title,
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
                    Title = item.Title,
                    OwnerId = item.OwnerId,
                    Owner = item.Owner,
                    ImageName = item.ImageName
                });
            }

            return new JsonResult(result);
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
