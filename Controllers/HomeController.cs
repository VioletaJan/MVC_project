using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using firstMVC.Models;
using System.Data;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace firstMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;
    private CustomerModel _customerModel;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _customerModel = new CustomerModel(_configuration);
    }

    //********************************'''

    public IActionResult Index()


    {
        ViewBag.Username = HttpContext.Session.GetString(SessionVariables.SessionKeyUsername);
        ViewBag.SessionId = HttpContext.Session.GetString(SessionVariables.SessionKeySessionId);

        _logger.LogInformation("Username:{session}", HttpContext.Session.GetString(SessionVariables.SessionKeyUsername));
        _logger.LogInformation("SessionId:{session}", HttpContext.Session.GetString(SessionVariables.SessionKeySessionId));

     
        return View();

    }

    //*************************************Show DesinfoSpridare table*****************************


    public IActionResult Index2()
    {
        ViewBag.Username = HttpContext.Session.GetString(SessionVariables.SessionKeyUsername);
        ViewBag.SessionId = HttpContext.Session.GetString(SessionVariables.SessionKeySessionId);

        _logger.LogInformation("Username:{session}", HttpContext.Session.GetString(SessionVariables.SessionKeyUsername));
        _logger.LogInformation("SessionId:{session}", HttpContext.Session.GetString(SessionVariables.SessionKeySessionId));

        CustomerModel desinfoSpridModel = new CustomerModel(_configuration);
        DataTable desinfoSpridareData = desinfoSpridModel.GetDesinfoSpridare(); //Vi kan skicka in PARAMETER "name" för att kunna söka efter namn (i CustomerModel vi moste skriva in parameter  GetDesinfospridare(string name)Mer info : 12:13 on sista video
        ViewBag.DesinfoSpridare = desinfoSpridareData;
        return View();

    }

    //************************************* FRITEXTFÄLT för sökning efter namn *****************************

    public IActionResult SearchCustomer(string name)
    {
        CustomerModel dm2 = new CustomerModel(_configuration);
        DataTable desSpridData = dm2.SearchCustomer(name);
        ViewBag.SearchResults = desSpridData;
        ViewBag.namn = name;
        return View();

    }

    //**************************************** FORMULÄR för att lägga till data***************************************************


    public IActionResult AddAgent(string namn, string nr, string specialite, string namnInc, string nrInc)
    {

        _customerModel.AddAgent(namn, nr, specialite, namnInc, nrInc);
        return RedirectToAction("Index2");

    }

    //********************************* RAPPORT tabellen ******************************************************
    public IActionResult Repport()
    {
        CustomerModel dm4 = new CustomerModel(_configuration);
        DataTable repportData = dm4.Repport();
        ViewBag.RepportResults = repportData;

        //this is from DROPDOWN funktion ChooseTitel.
        //check if TempData is not null and if it true - show the result on the Repport page
        if (TempData["title"] != null)
        {
            DataTable repportData2 = _customerModel.ChooseTitel(TempData["title"].ToString());
            ViewBag.TitelSearch = repportData2;
        }

        return View();
    }

    //********************************* DELETE link *********************************

    public IActionResult DeleteAgent(string namn, string nr)
    {
        _customerModel.DeleteAgent(namn, nr);
        return RedirectToAction("Index2");
    }

    //********************************* DROPDOWN Rapport PROCEDURE ***************************

    public IActionResult ChooseTitel(string Choose)
    {
        //TempData temporarily saves data and deletes it automatically after a value is recovered
        TempData["title"] = Choose;  //choose variable comes from Views i Rapport. It's the name for the data we choose to select
        return RedirectToAction("Repport");
    }


    //************************************* Show Fält Rapport table *****************************


    public IActionResult FaltRapport()
    {
        CustomerModel desinfoSpridModel = new CustomerModel(_configuration);
        DataTable faltRapportData = desinfoSpridModel.FaltRapport();
        ViewBag.AvslRapFältrapport = faltRapportData;
        return View();
    }

    //********************************* PROCEDURE to move report to field report table **************


    public IActionResult MoveReport(string datumIn, string titelIn, string typNrIn)
    {
        _customerModel.MoveReport(datumIn, titelIn, typNrIn);
        return RedirectToAction("FaltRapport");
    }


    //********************************* LOGIN and LOGOUT***********************

    public IActionResult Login(string username)
    {
        HttpContext.Session.SetString(SessionVariables.SessionKeyUsername, username);
        HttpContext.Session.SetString(SessionVariables.SessionKeySessionId, Guid.NewGuid().ToString());

        _logger.LogInformation("Username:{session}", HttpContext.Session.GetString(SessionVariables.SessionKeyUsername));
        _logger.LogInformation("SessionId:{session}", HttpContext.Session.GetString(SessionVariables.SessionKeySessionId));

        return RedirectToAction("Index2");
    }


    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        _logger.LogInformation("Username:{session}", HttpContext.Session.GetString(SessionVariables.SessionKeyUsername));
        _logger.LogInformation("SessionId:{session}", HttpContext.Session.GetString(SessionVariables.SessionKeySessionId));

        return RedirectToAction("Index");
    }

    public static class SessionVariables
    {
        public const string SessionKeyUsername = "Username";
        public const string SessionKeySessionId = "SessionId";
       
    }

    

    //***********************************************

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}   

