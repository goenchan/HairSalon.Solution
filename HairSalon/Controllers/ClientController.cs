using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using HairSalon.Models;

namespace HairSalon.Controllers
{
  public class ClientController : Controller
  {
    [HttpGet("/clients")]
    public ActionResult Index()
    {
      Dictionary<string, object> model = new Dictionary <string, object>();
      List<Client> allClients = Client.GetAll();
      List<Stylist> allStylists = Stylist.GetAll();
      model.Add("allClients", allClients);
      model.Add("allStylists", allStylists);
      return View(model);
    }

    [HttpGet("clients/new")]
    public ActionResult CreateForm()
    {
      Dictionary<string, object> model = new Dictionary <string, object>();
      List<Stylist> allStylists = Stylist.GetAll();
      List<Client> allClients = Client.GetAll();
      model.Add("allStylists",allStylists);
      model.Add("allClients",allClients);
      return View(model);
    }

    [HttpPost("/clients")]
    public ActionResult Create(int clientStylistId, string clientName)
    {
      Client newClient = new Client(clientName);
      newClient.Save();
      Stylist foundStylist = Stylist.Find(clientStylistId);
      newClient.AddStylist(foundStylist);

      return RedirectToAction("Index");
    }

    [HttpGet("/clients/delete")]
    public ActionResult DeleteAll()
    {
      Stylist.DeleteAll();
      return RedirectToAction("Index");
    }

    [HttpGet("clients/{clientId}")]
    public ActionResult Details(int clientId)
    {
      Dictionary<string, object> model = new Dictionary <string, object>();
      Client selectedClient = Client.Find(clientId);
      List<Stylist> clientStylists = selectedClient.GetStylists();
      List<Stylist> allStylists = Stylist.GetAll();
      model.Add("selectedClient", selectedClient);
      model.Add("clientStylists", clientStylists);
      model.Add("allStylists", allStylists);
      return View(model);
    }

    [HttpPost("/clients/{clientId}")]
    public ActionResult AddClient(int StylistId, int clientId)
    {
      Stylist foundStylist = Stylist.Find(StylistId);
      Client foundClient = Client.Find(clientId);
      foundClient.AddStylist(foundStylist);
      return RedirectToAction("Details", new {clientId = foundClient.id});
    }


    [HttpGet("/clients/{clientId}/update")]
    public ActionResult UpdateForm(int clientId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      List<Stylist> allStylists = Stylist.GetAll();
      Client selectedClient = Client.Find(clientId);
      model.Add("allStylists", allStylists);
      model.Add("client", selectedClient);
      return View(model);
    }

    [HttpPost("/clients/{clientId}/update")]
    public ActionResult Update(int clientId, string newName)
    {
      Client thisClient = Client.Find(clientId);
      thisClient.Edit(newName);
      return RedirectToAction("Details", new {clientId = thisClient.id});
    }

    [HttpGet("/clients/{clientId}/delete")]
    public ActionResult Delete(int clientId)
    {
      Client thisClient = Client.Find(clientId);
      Client.Delete(clientId);
      return RedirectToAction("Index");
    }

    [HttpPost("/clients/search")]
    public ActionResult Search()
    {
      string userInput = Request.Form["searched"];
      string searchedClient =
      char.ToUpper(userInput[0]) + userInput.Substring(1);
      List<Client> foundClients = Client.SearchClient(searchedClient);
      return View(foundClients);
    }
  }
}
