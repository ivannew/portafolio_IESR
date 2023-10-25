using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using portafolio_IESR.Models;

namespace portafolio_IESR.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IConfiguration _configuration;

        public ClientesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            List<ClienteModel> clientes = new List<ClienteModel>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GetClientes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ClienteModel cliente = new ClienteModel
                            {
                                Id = Convert.ToInt32(rdr["Id"]),
                                NombreEmpresa = rdr["NombreEmpresa"].ToString(),
                                NumeroTelefono = rdr["NumeroTelefono"].ToString(),
                                Correo = rdr["Correo"].ToString()
                            };
                            clientes.Add(cliente);
                        }
                    }
                }
            }

            return View(clientes);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ClienteModel cliente)
        {
            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("InsertCliente", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NombreEmpresa", cliente.NombreEmpresa);
                        cmd.Parameters.AddWithValue("@NumeroTelefono", cliente.NumeroTelefono);
                        cmd.Parameters.AddWithValue("@Correo", cliente.Correo);
                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        public IActionResult Edit(int id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            ClienteModel cliente = new ClienteModel();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GetClienteById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            cliente.Id = Convert.ToInt32(rdr["Id"]);
                            cliente.NombreEmpresa = rdr["NombreEmpresa"].ToString();
                            cliente.NumeroTelefono = rdr["NumeroTelefono"].ToString();
                            cliente.Correo = rdr["Correo"].ToString();
                        }
                    }
                }
            }

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ClienteModel cliente)
        {
            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("UpdateCliente", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", cliente.Id);
                        cmd.Parameters.AddWithValue("@NombreEmpresa", cliente.NombreEmpresa);
                        cmd.Parameters.AddWithValue("@NumeroTelefono", cliente.NumeroTelefono);
                        cmd.Parameters.AddWithValue("@Correo", cliente.Correo);
                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        public IActionResult Delete(int id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DeleteCliente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
