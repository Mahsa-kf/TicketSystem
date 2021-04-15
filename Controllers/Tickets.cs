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

        [HttpGet]
        public IActionResult Create()
        {
            var ticket = new Models.Ticket();
            return View(ticket);
        }

        [HttpPost]
        public IActionResult Create(Models.Ticket t)
        {
            //load tickets.xml
            string path = Request.PathBase + "App_Data/tickets.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                //if file exists, just load it and create new ticket
                doc.Load(path);

                //create a new ticket
                XmlElement ticketXmlElement = _CreateTicketElement(doc, t);

                // get the root element
                doc.DocumentElement.AppendChild(ticketXmlElement);

            }
            else
            {
                //file doesn't exist, so create and create new ticket
                XmlNode dec = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                doc.AppendChild(dec);
                XmlNode root = doc.CreateElement("tickets");

                //create a new ticket
                XmlElement ticketXmlElement = _CreateTicketElement(doc, t);
                root.AppendChild(ticketXmlElement);
                doc.AppendChild(root);
            }
            doc.Save(path);

            return View();
        }


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


        private XmlElement _CreateTicketElement(XmlDocument doc, Models.Ticket userInput)
        {
            XmlElement ticketXmlElement = doc.CreateElement("ticket");

            XmlAttribute idAttribute = doc.CreateAttribute("id");
            idAttribute.Value = GetNewTicketID().ToString();
            ticketXmlElement.Attributes.Append(idAttribute);


            XmlNode issueDateTimeXmlElement = doc.CreateElement("issueDateTime");
            issueDateTimeXmlElement.InnerText = userInput.IssueDateTime.ToString();
            ticketXmlElement.AppendChild(issueDateTimeXmlElement);

            XmlNode statusXmlElement = doc.CreateElement("status");
            statusXmlElement.InnerText = userInput.Status.ToString();
            ticketXmlElement.AppendChild(statusXmlElement);


            XmlNode priorityXmlElement = doc.CreateElement("priority");
            priorityXmlElement.InnerText = userInput.Priority.ToString();
            ticketXmlElement.AppendChild(priorityXmlElement);


            XmlNode creatorIdXmlElement = doc.CreateElement("creatorId");
            creatorIdXmlElement.InnerText = userInput.CreatorId.ToString();
            ticketXmlElement.AppendChild(creatorIdXmlElement);


            XmlNode subjectXmlElement = doc.CreateElement("subject");
            subjectXmlElement.InnerText = userInput.Subject.ToString();
            ticketXmlElement.AppendChild(subjectXmlElement);

            XmlNode descriptionXmlElement = doc.CreateElement("description");
            descriptionXmlElement.InnerText = userInput.Description.ToString();
            ticketXmlElement.AppendChild(descriptionXmlElement);


            XmlNode messagesXmlElement = doc.CreateElement("messages");
            XmlNode messageXmlElement = doc.CreateElement("message");


            XmlNode writerIdXmlElement = doc.CreateElement("writerId");
            writerIdXmlElement.InnerText = userInput.Messages.FirstOrDefault().WriterId.ToString();
            messageXmlElement.AppendChild(writerIdXmlElement);
           
            XmlNode dateTimeXmlElement = doc.CreateElement("dateTime");
            issueDateTimeXmlElement.InnerText = userInput.Messages.FirstOrDefault().DateTime.ToString();
            messageXmlElement.AppendChild(issueDateTimeXmlElement);

            XmlNode textXmlElement = doc.CreateElement("text");
            textXmlElement.InnerText = userInput.Messages.FirstOrDefault().Text.ToString();
            messageXmlElement.AppendChild(textXmlElement);

            messagesXmlElement.AppendChild(messageXmlElement);

           ticketXmlElement.AppendChild(messagesXmlElement);

            return ticketXmlElement;
        }

        private int GetNewTicketID()
        {
            // Gett the list of the tickets and find the last id
            IList<Models.Ticket> ticketList = GetTickets();
            var lastId = ticketList.Max(ticket => ticket.Id);
            return lastId + 1;
        }

    }
}
