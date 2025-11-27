using System;
using System.Text.Json;
using System.IO;

namespace Projeto_lista_de_afazeres
{
    class Program
    {
        static List<TaskItem> tarefas = new List<TaskItem>(); // cria uma lista com base na classe "TaskItem"
        static void SalvarListaEmJSON() //cria o arquivo "Tarefas.json"
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

        static bool SolucaoErro(int variavel) //cria uma função para solução de erros de indice inválido
        {
            if (variavel < 0 || variavel >= tarefas.Count)
            {
                Console.WriteLine("Indice invalido");
                return false;
            }
            return true;
        }

        static bool SolucaoTry(out int numero) //outra solução de erro para se caso o campo nao for um numero
        {
            bool deuCerto = int.TryParse(Console.ReadLine(), out numero);

            if (!deuCerto)
            {
                Console.WriteLine("Digite um número válido.");
            }

            return deuCerto;
        }
        //a função para adicionar uma tarefa
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
        //a função criada para listar as tarefas
        static void ListarTarefas(bool pausarNoFinal = true)
        {
            Console.WriteLine("tarefas: ");
            if (tarefas.Count < 1)
            {
                Console.WriteLine("Você não tem nenhuma tarefa!");

                if (pausarNoFinal) //se o parametro dado for true, ele cai dentro desse if e faz a pausa
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
        //função pra edtitar as tarefas (trocar nome) 
        static void EditarTarefa()
        {
            Console.WriteLine("Editando tarefa...");

            ListarTarefas(false);
            if (tarefas.Count == 0) return;

            Console.WriteLine("Escolha a tarefa: ");
            int editar;
            if (!SolucaoTry(out editar)) return;

            editar -= 1;
            if (!SolucaoErro(editar))   //solução de erro que verifica se a variavel passada como parametro é um numero
            {
                return;
            }
            Console.WriteLine($"Novo nome para a tarefa: (nome antigo {tarefas[editar].Nome})");
            tarefas[editar].Nome = Console.ReadLine();
            SalvarListaEmJSON();
            Console.WriteLine("Tarefa editada! Pressione qualquer botao para voltar...");
            Console.ReadKey();
        }

        //função para concluir uma tarefa existente

        static void ConcluirTarefa()
        {
            ListarTarefas(false); //false para tirar a pausa da função
            if (tarefas.Count == 0) return;

            Console.WriteLine("Escolha sua tarefa: ");

            int conclu;
            if (!SolucaoTry(out conclu)) return; //verificação se a variavel inserida é um numero

            conclu -= 1;
            if (!SolucaoErro(conclu)) return;
            tarefas[conclu].Concluida = true;
            SalvarListaEmJSON();
            Console.WriteLine($"{tarefas[conclu].Nome} - concluida");
            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
        }

        //função pra exlcuir uma tarefa existente
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