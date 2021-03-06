using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using BestRestaurants.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BestRestaurants.Controllers
{
    
    public class RestaurantsController : Controller
    {
        private readonly BestRestaurantsContext _db;
        public RestaurantsController(BestRestaurantsContext db)
        {
            _db = db;
        }
        public ActionResult Index()
        {
            List<Restaurant> model = _db.Restaurants.ToList();
            return View(model);
        }
        public ActionResult Create()
        {
            ViewBag.CuisineId = new SelectList(_db.Cuisines, "CuisineId", "CuisineName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Restaurant restaurant )
        {
            _db.Restaurants.Add(restaurant);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            Restaurant restaurant =  _db.Restaurants.FirstOrDefault(x => x.RestaurantId == id);
            List<Review> reviews = _db.Reviews.Where(x => x.RestaurantId == id).ToList();
            Dictionary<string, object> model = new Dictionary<string, object>();
            model.Add("restaurant", restaurant);
            model.Add("reviews", reviews);
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            Restaurant restraurant = _db.Restaurants.FirstOrDefault(x => x.RestaurantId == id);
            _db.Restaurants.Remove(restraurant);
            _db.SaveChanges();
            return RedirectToAction("Index");  
        }
        public ActionResult Edit(int id)
        {
            ViewBag.CuisineId = new SelectList(_db.Cuisines, "CuisineId", "CuisineName");
            var thisRestaurant = _db.Restaurants.FirstOrDefault(restaurants => restaurants.RestaurantId == id);
            return View(thisRestaurant);
        }

        [HttpPost]
        public ActionResult Edit(Restaurant restaurant)
        {
            _db.Entry(restaurant).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Error()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string restaurantName)
        {
        List<Restaurant> model = _db.Restaurants.Where(x => x.RestaurantName.Contains(restaurantName)).ToList();
        return View("Index", model);
        }
    }
}