using HouseM8API.Helpers;
using HouseM8API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;

namespace HouseM8API.Controllers
{
    /// <summary>
    /// Controler para obter o Endereço através das coordenadas GPS
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        /// <summary>
        /// Rota para obter o endereço através de coordenadas GPS
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lng">Longitude</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /location?lat=41.3085168113476&amp;lng=-8.344125348737023
        /// 
        /// </remarks>
        /// <returns>Endereço ou Exception</returns>
        /// <response code="200">Retorna o Endereço relacionado com as coordenadas</response>
        /// <response code="422">Unprocessable Entity</response>
        [HttpGet]
        [Authorize(Roles = "M8,EMPLOYER")]
        public IActionResult GetLocation(double lat, double lng)
        {
            try
            {
                string address = DistancesHelper.getAddressFromCoordinates(lat.ToString(new CultureInfo("en-US")), lng.ToString(new CultureInfo("en-US")));
                return Ok(new { address });
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(new ErrorMessageModel(ex.Message));
            }

        }
    }
}
