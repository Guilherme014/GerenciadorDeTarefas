using System;
using System.Text.Json;
using System.IO;

namespace Projeto_lista_de_afazeres
{
    class Program
    {
        static List<TaskItem> tarefas = new List<TaskItem>();
        static void SalvarListaEmJSON()
        {
            var json = JsonSerializer.Serialize(tarefas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("Tarefas.json", json);
        }

        static void CarregarListaDeJSON()
        {
            try
            {
                if (File.Exists("Tarefas.json"))
                {
                    string json = File.ReadAllText("Tarefas.json");
                    tarefas = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                
            }
        }

        static bool SolucaoErro(int variavel)
        {
            if (variavel < 0 || variavel >= tarefas.Count)
            {
                Console.WriteLine("Indice invalido");
                return false;
            }
            return true;
        }

        static bool SolucaoTry(out int numero)
        {
            bool deuCerto = int.TryParse(Console.ReadLine(), out numero);

            if (!deuCerto)
            {
                Console.WriteLine("Digite um número válido.");
            }

            return deuCerto;
        }

        static void AdicionarTarefa()
        {
            Console.Write("Nome da tarefa: ");
            string nome = Console.ReadLine();

            TaskItem nova = new TaskItem()
            {
                Nome = nome,
                Concluida = false
            };
            tarefas.Add(nova);
            SalvarListaEmJSON();
            Console.WriteLine("Nova tarefa adicionada! Pressione qualquer tecla...");
            Console.ReadKey();

        }

        static void ListarTarefas(bool pausarNoFinal = true)
        {
            Console.WriteLine("tarefas: ");
            if (tarefas.Count < 1)
            {
                Console.WriteLine("Você não tem nenhuma tarefa!");

                if (pausarNoFinal)
                {
                    Console.WriteLine("Pressione qualquer botao para continuar...");
                    Console.ReadKey();
                }
                return;
            }
            int contador = 0;
            foreach (var t in tarefas)
            {
                contador++;
                Console.WriteLine($"{contador} - {t.Nome}  (concluido? - {t.Concluida})");
            }
            if (pausarNoFinal)
            {
                Console.WriteLine("Pressione qualquer botao para continuar...");
                Console.ReadKey();
            }

           
        }

        static void EditarTarefa()
        {
            Console.WriteLine("Editando tarefa...");

            ListarTarefas(false);
            if (tarefas.Count == 0) return;

            Console.WriteLine("Escolha a tarefa: ");
            int editar;
            if (!SolucaoTry(out editar)) return;

            editar -= 1;
            if (!SolucaoErro(editar))
            {
                return;
            }
            Console.WriteLine($"Novo nome para a tarefa: (nome antigo {tarefas[editar].Nome})");
            tarefas[editar].Nome = Console.ReadLine();
            SalvarListaEmJSON();
            Console.WriteLine("Tarefa editada! Pressione qualquer botao para voltar...");
            Console.ReadKey();
        }

        static void ConcluirTarefa()
        {
            ListarTarefas(false);
            if (tarefas.Count == 0) return;

            Console.WriteLine("Escolha sua tarefa: ");

            int conclu;
            if (!SolucaoTry(out conclu)) return;

            conclu -= 1;
            if (!SolucaoErro(conclu)) return;
            tarefas[conclu].Concluida = true;
            SalvarListaEmJSON();
            Console.WriteLine($"{tarefas[conclu].Nome} - concluida");
            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
        }

    
        static void ExcluirTarefa()
        {
            ListarTarefas(false);
            if (tarefas.Count == 0) return;
            Console.WriteLine("Escolha uma tarefa para deletar: ");
            int exclu;
            if (!SolucaoTry(out exclu)) return;
            exclu -= 1;
            if (!SolucaoErro(exclu))
            {
                return;
            }
            Console.WriteLine("Aviso: Sua tarefa sera removida permanentemente, você tem certeza que deseja deletar? ");
            Console.WriteLine("confirmar S/N");
            var tecla = Console.ReadKey(true);
            if (tecla.Key == ConsoleKey.S)
            {
                Console.WriteLine("Confirmado! Deletando tarefa...");
                tarefas.RemoveAt(exclu);
                SalvarListaEmJSON();
                Console.WriteLine("Tarefa deletada");
            }
            else
            {
                Console.WriteLine("Cancelado....");
            }

        }

        static void Main()
        {

            CarregarListaDeJSON();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("===MENU===");
                Console.WriteLine("1 - Adicionar tarefa");
                Console.WriteLine("2 - Listar tarefas");
                Console.WriteLine("3 - Editar tarefa");
                Console.WriteLine("4 - Concluir tarefa");
                Console.WriteLine("5 - Excluir tarefa");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha: ");

                string opc = Console.ReadLine();

                switch (opc)
                {
                    case "1":
                        AdicionarTarefa();
                        break;

                    case "2":
                        ListarTarefas();
                        break;

                    case "3":
                        EditarTarefa();
                            break;  

                    case "4":
                        ConcluirTarefa();
                        break;

                    case "5":
                        ExcluirTarefa();
                        break;

                    case "0":
                        return;
                }
            }

            
        

        }
    }
}