using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    public class Tickets : Controller
    {

        public IActionResult Index()
        {
            IList<Ticket> ticketList = GetTickets();
            return View(ticketList);
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    var book = new Models.Ticket();
        //    return View(book);
        //}

        //[HttpPost]
        //public IActionResult Create(Models.Ticket t)
        //{
        //    //load tickets.xml
        //    string path = Request.PathBase + "App_Data/tickets.xml";
        //    XmlDocument doc = new XmlDocument();

        //    if (System.IO.File.Exists(path))
        //    {
        //        //if file exists, just load it and create new ticket
        //        doc.Load(path);

        //        //create a new ticket
        //        XmlElement ticketXmlElement = _CreateTicketElement(doc, t);

        //        // get the root element
        //        doc.DocumentElement.AppendChild(ticketXmlElement);

        //    }
        //    else
        //    {
        //        //file doesn't exist, so create and create new ticket
        //        XmlNode dec = doc.CreateXmlDeclaration("1.0", "utf-8", "");
        //        doc.AppendChild(dec);
        //        XmlNode root = doc.CreateElement("tickets");

        //        //create a new ticket
        //        XmlElement ticketXmlElement = _CreateTicketElement(doc, t);
        //        root.AppendChild(ticketXmlElement);
        //        doc.AppendChild(root);
        //    }
        //    doc.Save(path);

        //    return View();
        //}


        private IList<Ticket> GetTickets()
        {
            IList<Ticket> ticketList = new List<Ticket>();
            //Define the file location
            string path = Request.PathBase + "App_Data/tickets.xml";

            if (System.IO.File.Exists(path))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                XmlNodeList tickets = xmlDoc.GetElementsByTagName("ticket");

                foreach (XmlElement t in tickets)
                {
                    Ticket newTicket = new Ticket();

                    newTicket.Id = Int32.Parse(t.Attributes.GetNamedItem("id").InnerText);
                    newTicket.IssueDateTime = DateTime.Parse(t.GetElementsByTagName("issueDateTime")[0].InnerText);
                    newTicket.Status = t.GetElementsByTagName("status")[0].InnerText;
                    newTicket.Priority = t.GetElementsByTagName("priority")[0].InnerText;
                    newTicket.CreatorId = Int32.Parse(t.GetElementsByTagName("creatorId")[0].InnerText);
                    newTicket.Subject = t.GetElementsByTagName("subject")[0].InnerText;
                    newTicket.Description = t.GetElementsByTagName("description")[0].InnerText;

                    List<Message> messageList = new List<Message>();
                    XmlNodeList messages = t.GetElementsByTagName("message");

                    foreach (XmlElement m in messages)
                    {
                        Message newMessage = new Message();
                        newMessage.WriterId = Int32.Parse(m.GetElementsByTagName("writerId")[0].InnerText);
                        newMessage.DateTime = DateTime.Parse(t.GetElementsByTagName("dateTime")[0].InnerText);
                        newMessage.Text = m.GetElementsByTagName("text")[0].InnerText;

                        messageList.Add(newMessage);
                    }

                    newTicket.Messages = messageList;

                    ticketList.Add(newTicket);
                }
            }
            return ticketList;
        }


        //private XmlElement _CreateBooxElement(XmlDocument doc, Models.Book userInput)
        //{
        //    XmlElement bookXmlElement = doc.CreateElement("book");

        //    XmlNode idXmlElement = doc.CreateElement("id");
        //    idXmlElement.InnerText = GetNewBookID().ToString();
        //    bookXmlElement.AppendChild(idXmlElement);

        //    XmlNode titleXmlElement = doc.CreateElement("title");
        //    titleXmlElement.InnerText = userInput.Title;
        //    bookXmlElement.AppendChild(titleXmlElement);

        //    XmlNode authorXmlElement = doc.CreateElement("author");

        //    XmlAttribute titleAttribute = doc.CreateAttribute("title");
        //    titleAttribute.Value = userInput.Author.Title;
        //    authorXmlElement.Attributes.Append(titleAttribute);

        //    XmlNode firstNameXmlElement = doc.CreateElement("firstname");
        //    firstNameXmlElement.InnerText = userInput.Author.FirstName;
        //    authorXmlElement.AppendChild(firstNameXmlElement);

        //    XmlNode middleNameXmlElement = doc.CreateElement("middlename");
        //    middleNameXmlElement.InnerText = userInput.Author.MiddleName;
        //    authorXmlElement.AppendChild(middleNameXmlElement);

        //    XmlNode lastNameXmlElement = doc.CreateElement("lastname");
        //    lastNameXmlElement.InnerText = userInput.Author.LastName;
        //    authorXmlElement.AppendChild(lastNameXmlElement);

        //    bookXmlElement.AppendChild(authorXmlElement);

        //    return bookXmlElement;
        //}

        //private int GetNewBookID()
        //{
        //    // Gett the list of the books and find the last id
        //    IList<Models.Book> bookList = GetBooks();
        //    var lastId = bookList.Max(bookList => bookList.Id);
        //    return lastId + 1;
        //}

//    }
//}
}
}
