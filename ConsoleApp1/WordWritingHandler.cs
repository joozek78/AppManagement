using System.Threading.Tasks;
using ConsoleApp1.Words;
using Microsoft.AspNetCore.Http;

namespace ConsoleApp1
{
    public class WordWritingHandler
    {
        private readonly IWordProvider WordProvider;

        public WordWritingHandler(IWordProvider wordProvider)
        {
            WordProvider = wordProvider;
        }

        public Task Handle(HttpContext context) => context.Response.WriteAsync($"Response from {nameof(WordWritingHandler)}: {WordProvider.Word}");
    }
}