using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.IO;
using WebApiTodo.Models;

namespace WebApiTodo.Controllers
{
    [RoutePrefix("Api/Todo")]
    public class TodoController : ApiController
    {
        DbTodoEntities objEntity = new DbTodoEntities();

        [HttpGet]
        [Route("AllTodos")]
        public IQueryable<Todo> GetTodos()
        {
            try
            {
                return objEntity.Todoes;  
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetTodo/{Id}")]
        public IHttpActionResult GetTodo(string id)
        {
            Todo objTodo = new Todo();
            int IDTodo = Convert.ToInt32(id);
            try
            {
                objTodo = objEntity.Todoes.Find(IDTodo);
                if (objTodo == null)
                {
                    return NotFound();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return Ok(objTodo);
        }

        [HttpPost]
        [Route("CreateTodo")]
        public HttpResponseMessage CreateTodo()
        {
            try
            {
                string extension = null;
                int id = -1;
                var httpRequest = HttpContext.Current.Request;
                //Upload Image
                var postedFile = httpRequest.Files["Image"];

                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(httpRequest.Files["Image"].InputStream))
                {
                    fileData = binaryReader.ReadBytes(httpRequest.Files["Image"].ContentLength);
                }

                extension = Path.GetExtension(postedFile.FileName);
                int.TryParse(HttpContext.Current.Request.Params["Id"], out id);

                Todo data = new Todo()
                {
                    Description = HttpContext.Current.Request.Params["Description"],
                    Status = "P",
                    AttachType = extension,
                    AttachFile = fileData
                };
                objEntity.Todoes.Add(data);
                objEntity.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created);
                
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        [HttpPut]
        [Route("UpdateTodo")]
        public HttpResponseMessage UpdateTodo()
        {
            try
            {
                string extension = null;
                int id = -1;
                var httpRequest = HttpContext.Current.Request;
                //Upload Image
                var postedFile = httpRequest.Files["Image"];

                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(httpRequest.Files["Image"].InputStream))
                {
                    fileData = binaryReader.ReadBytes(httpRequest.Files["Image"].ContentLength);
                }

                extension = Path.GetExtension(postedFile.FileName);
                int.TryParse(HttpContext.Current.Request.Params["TodoId"], out id);

                Todo objTodo = new Todo();
                objTodo = objEntity.Todoes.Find(id);
                if (objTodo != null)
                {
                    objTodo.Description = HttpContext.Current.Request.Params["Description"];
                    objTodo.Status = HttpContext.Current.Request.Params["Status"];
                    objTodo.AttachType = extension;
                    objTodo.AttachFile = fileData;
                }
                int i = this.objEntity.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
                
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        [HttpPut]
        [Route("CompleteTodo")]
        public IHttpActionResult CompleteTodo(int id)
        { 
            Todo todo = objEntity.Todoes.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            if (todo != null)
            {
                todo.Status = "C";
            }
            int i = this.objEntity.SaveChanges();

            return Ok(todo);
        }

        [HttpDelete]
        [Route("DeleteTodo")]
        public IHttpActionResult DeleteTodo(int id)
        {
            //int empId = Convert.ToInt32(id);  
            Todo todo = objEntity.Todoes.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            objEntity.Todoes.Remove(todo);
            objEntity.SaveChanges();

            return Ok(todo);
        }

    }
}
