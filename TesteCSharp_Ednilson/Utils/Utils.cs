using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TesteCSharp_Ednilson.Utils
{
    public static class Utils
    {
        public static DataTable DtbEstados()
        {
            var dtEstados = new DataTable();
            dtEstados.Columns.Add("Estado");
            dtEstados.Columns.Add("UF");

            dtEstados.Rows.Add("Acre", "AC");
            dtEstados.Rows.Add("Alagoas", "AL");
            dtEstados.Rows.Add("Amapá", "AP");
            dtEstados.Rows.Add("Amazonas", "AM");
            dtEstados.Rows.Add("Bahia", "BA");
            dtEstados.Rows.Add("Ceará", "CE");
            dtEstados.Rows.Add("Distrito Federal", "DF");
            dtEstados.Rows.Add("Espírito Santo", "ES");
            dtEstados.Rows.Add("Goiás", "GO");
            dtEstados.Rows.Add("Maranhão", "MA");
            dtEstados.Rows.Add("Mato Grosso", "MT");
            dtEstados.Rows.Add("Mato Grosso do Sul", "MS");
            dtEstados.Rows.Add("Minas Gerais", "MG");
            dtEstados.Rows.Add("Pará", "PA");
            dtEstados.Rows.Add("Paraíba", "PB");
            dtEstados.Rows.Add("Paraná", "PR");
            dtEstados.Rows.Add("Pernambuco", "PE");
            dtEstados.Rows.Add("Piauí", "PI");
            dtEstados.Rows.Add("Rio de Janeiro", "RJ");
            dtEstados.Rows.Add("Rio Grande do Norte", "RN");
            dtEstados.Rows.Add("Rio Grande do Sul", "RS");
            dtEstados.Rows.Add("Rondônia", "RO");
            dtEstados.Rows.Add("Roraima", "RR");
            dtEstados.Rows.Add("Santa Catarina", "SC");
            dtEstados.Rows.Add("São Paulo", "SP");
            dtEstados.Rows.Add("Sergipe", "SE");
            dtEstados.Rows.Add("Tocantins", "TO");

            return dtEstados;
        }

        public static string GetEstado(string Uf)
        {

            var dtbEstados = DtbEstados();
            
            var consulta = from p in dtbEstados.AsEnumerable()
                           where p.Field<string>("UF") == Uf
                           select new {estado = p.Field<string>("Estado")};

            return consulta.FirstOrDefault().estado;
        }


        public static bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");


            // var cpfLong = Convert.ToUInt64(cpf);
            // var cpfLongSt = cpfLong.ToString();
            // 
            // cpf = cpfLongSt.Substring(0, 11);

            cpf = Convert.ToUInt64(cpf).ToString(@"00000000000");


            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        public static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        public static bool isTipoPessoa(char TipoPessoa)
        {
            if (TipoPessoa != 'P' && TipoPessoa != 'F')
                return false;

            return true;
        }
    }
}