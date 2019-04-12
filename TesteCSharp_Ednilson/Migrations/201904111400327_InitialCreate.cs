namespace TesteCSharp_Ednilson.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Empresa",
                c => new
                    {
                        Cnpj = c.String(nullable: false, maxLength: 18, unicode: false),
                        NomeFantasia = c.String(nullable: false, maxLength: 50, unicode: false),
                        UF = c.String(nullable: false, maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => t.Cnpj);
            
            CreateTable(
                "dbo.Fornecedor",
                c => new
                    {
                        Cpf_Cnpj = c.String(nullable: false, maxLength: 18, unicode: false),
                        Empresa_Cnpj = c.String(maxLength: 18, unicode: false),
                        Tipo_Pessoa = c.String(nullable: false, maxLength: 1, unicode: false),
                        Nome = c.String(nullable: false, maxLength: 50, unicode: false),
                        Rg = c.String(maxLength: 14, unicode: false),
                        DataCadastro = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DataNascimento = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Cpf_Cnpj)
                .ForeignKey("dbo.Empresa", t => t.Empresa_Cnpj)
                .Index(t => t.Empresa_Cnpj);
            
            CreateTable(
                "dbo.Telefone",
                c => new
                    {
                        Numero = c.String(nullable: false, maxLength: 30, unicode: false),
                        Fornecedor_Cpf_Cnpj = c.String(nullable: false, maxLength: 18, unicode: false),
                    })
                .PrimaryKey(t => new { t.Numero, t.Fornecedor_Cpf_Cnpj })
                .ForeignKey("dbo.Fornecedor", t => t.Fornecedor_Cpf_Cnpj, cascadeDelete: true)
                .Index(t => t.Fornecedor_Cpf_Cnpj);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Telefone", "Fornecedor_Cpf_Cnpj", "dbo.Fornecedor");
            DropForeignKey("dbo.Fornecedor", "Empresa_Cnpj", "dbo.Empresa");
            DropIndex("dbo.Telefone", new[] { "Fornecedor_Cpf_Cnpj" });
            DropIndex("dbo.Fornecedor", new[] { "Empresa_Cnpj" });
            DropTable("dbo.Telefone");
            DropTable("dbo.Fornecedor");
            DropTable("dbo.Empresa");
        }
    }
}
