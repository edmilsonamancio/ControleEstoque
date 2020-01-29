using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace ControleEstoque.Web.Models
{
    public class EstadoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        [MaxLength(30, ErrorMessage = "O nome pode ter no máximo 30 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha a unidade federativa.")]
        [MaxLength(3, ErrorMessage = "O código internacional deve ter no máximo 3 caracteres.")]
        public string Uf { get; set; }

        public bool Ativo { get; set; }

        public int Id_Pais { get; set; }
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select count(*) from Estado";
                    ret = (int)comando.ExecuteScalar();
                }
            }

            return ret;
        }

        public static List<EstadoModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", int idPais = 0)
        {
            var ret = new List<EstadoModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    var pos = (pagina - 1) * tamPagina;

                    var filtroWhere = "";
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        filtroWhere = string.Format(" where lower(nome) like '%{0}%'", filtro.ToLower());
                    }

                    if (idPais > 0)
                    {
                        filtroWhere +=
                            (string.IsNullOrEmpty(filtroWhere) ? " where" : " and") +
                            string.Format(" id_pais = {0}", idPais);
                    }

                    var paginacao = "";
                    if (pagina > 0 && tamPagina > 0)
                    {
                        paginacao = string.Format(" offset {0} rows fetch next {1} row only",
                            pos > 0 ? pos : 0, tamPagina);
                    }

                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                        "select * " +
                        "from estado" + filtroWhere + " order by nome"+ paginacao);
                    var reader = comando.ExecuteReader();
                   
                    while (reader.Read())
                    {
                        ret.Add(new EstadoModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Uf = (string)reader["uf"],
                            Ativo = (bool)reader["ativo"]
                        });
                    }
                }
            }
            return ret;
        }

        public static EstadoModel RecuperarPeloId(int id)
        {
            EstadoModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from estado where (id = @id)";
                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new EstadoModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Uf = (string)reader["uf"],
                            Id_Pais = (int)reader["id_pais"],
                            Ativo = (bool)reader["ativo"]
                        };
                    }
                }
            }

            return ret;
        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            if (RecuperarPeloId(id) != null)
            {
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;
                        comando.CommandText = "delete from estado where id = @id";
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = id;
                        ret = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }

            return ret;
        }

        public int Salvar()
        {
            var ret = 0;

            var model = RecuperarPeloId(this.Id);

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    if (model == null)
                    {
                        comando.CommandText = "insert into estado (nome, uf, ativo, id_pais) values (@nome, @uf, @ativo, @id_pais); select convert(int, scope_identity())";
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@uf", SqlDbType.VarChar).Value = this.Uf;
                        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
                        comando.Parameters.Add("@id_pais", SqlDbType.Int).Value = this.Id_Pais;

                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = "update Estado set nome=@nome, uf=@uf, ativo=@ativo, id_pais=@id_pais where id=@id";
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@uf", SqlDbType.VarChar).Value = this.Uf;
                        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
                        comando.Parameters.Add("@id_pais", SqlDbType.Int).Value = this.Id_Pais;
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;

                        if (comando.ExecuteNonQuery() > 0)
                        {
                            ret = this.Id;
                        }
                    }
                }
            }

            return ret;
        }
    }
}