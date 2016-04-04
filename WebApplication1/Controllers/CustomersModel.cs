using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KIS.App_Code;

namespace KIS.Models
{
    public class CustomerModel
    {
        public CustomerModel(String id)
        {
            Cliente customer = new Cliente(id);
            if (customer != null && customer.CodiceCliente.Length > 0)
            {
                this._CodiceCliente = customer.CodiceCliente;
                this._RagioneSociale = customer.RagioneSociale;
                this._PartitaIVA = customer.PartitaIVA;
                this._CodiceFiscale = customer.CodiceFiscale;
                this._Indirizzo = customer.Indirizzo;
                this._Citta = customer.Citta; 
                this._Provincia = customer.Provincia;
                this._CAP = customer.CAP;
                this._Stato = customer.Stato;
                this._Telefono = customer.Telefono;
                this._Email = customer.Email;
            }
        }

        private String _CodiceCliente;
        public String CodiceCliente
        {
            get
            {
                return this._CodiceCliente;
            }
        }

        private String _RagioneSociale;
        public String RagioneSociale
        {
            get
            {
                return this._RagioneSociale;
            }
        }

        private String _PartitaIVA;
        public String PartitaIVA
        {
            get
            {
                return this._PartitaIVA;
            }
            set
            {
                Cliente cust = new Cliente(this.CodiceCliente);
                if (cust != null && cust.CodiceCliente.Length > 0)
                {
                    cust.PartitaIVA = value;
                }
            }
        }

        private String _CodiceFiscale;
        public String CodiceFiscale
        {
            get
            {
                return this._CodiceFiscale;
            }
            set
            {
                Cliente cust = new Cliente(this.CodiceCliente);
                if (cust != null && cust.CodiceCliente.Length > 0)
                {
                    cust.CodiceFiscale = value;
                } 
            }
        }

        private String _Indirizzo;
        public String Indirizzo
        {
            get
            {
                return this._Indirizzo;
            }
            set
            {
                Cliente cust = new Cliente(this.CodiceCliente);
                if (cust != null && cust.CodiceCliente.Length > 0)
                {
                    cust.Indirizzo = value;
                }
            }
        }

        private String _Citta;
        public String Citta
        {
            get
            {
                return this._Citta;
            }
            set
            {
                Cliente cust = new Cliente(this.CodiceCliente);
                if (cust != null && cust.CodiceCliente.Length > 0)
                {
                    cust.Citta = value;
                }
            }
        }

        private String _Provincia;
        public String Provincia
        {
            get
            {
                return this._Provincia;
            }
            set
            {
                Cliente cust = new Cliente(this.CodiceCliente);
                if (cust != null && cust.CodiceCliente.Length > 0)
                {
                    cust.Provincia = value;
                }
            }
        }

        private String _CAP;
        public String CAP
        {
            get
            {
                return this._CAP;
            }
            set
            {
                Cliente cust = new Cliente(this.CodiceCliente);
                if (cust != null && cust.CodiceCliente.Length > 0)
                {
                    cust.CAP = value;
                }
            }
        }

        private String _Stato;
        public String Stato
        {
            get
            {
                return this._Stato;
            }
            set
            {
                Cliente cust = new Cliente(this.CodiceCliente);
                if (cust != null && cust.CodiceCliente.Length > 0)
                {
                    cust.Stato = value;
                }
            }
        }

        private String _Telefono;
        public String Telefono
        {
            get
            {
                return this._Telefono;
            }
            set
            {
                Cliente cust = new Cliente(this.CodiceCliente);
                if (cust != null && cust.CodiceCliente.Length > 0)
                {
                    cust.Telefono = value;
                }
            }
        }

        private String _Email;
        public String Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                Cliente cust = new Cliente(this.CodiceCliente);
                if (cust != null && cust.CodiceCliente.Length > 0)
                {
                    cust.Email = value;
                } 
            }
        }

    }

    public class CustomersListModel
    {
        public List<CustomerModel> CustomerPortfolio;
        public CustomersListModel()
        {
            CustomerPortfolio = new List<CustomerModel>();
            PortafoglioClienti elencoCli = new PortafoglioClienti();
            for (int i = 0; i < elencoCli.Elenco.Count; i++)
            {
                CustomerPortfolio.Add(new CustomerModel(elencoCli.Elenco[i].CodiceCliente));
            }
        }
    }

    public class CustomerModelStruct
    {
        public CustomerModelStruct()
        {
        }

        public String CodiceCliente
        { get; set; }

        public String RagioneSociale
        { get; set; }
        public String PartitaIVA
        { get; set; }

        public String CodiceFiscale
            { get; set; }

        public String Indirizzo
        { get; set; }

        public String Citta
{ get; set; }

        public String Provincia
{ get; set; }

        public String CAP
        { get; set; }

        public String Stato
        { get; set; }

        public String Telefono
{ get; set; }

        public String Email
        { get; set; }

    }

}