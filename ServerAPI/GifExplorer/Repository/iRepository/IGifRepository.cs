using GifExplorer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GifExplorer.Repository.iRepository
{
    public interface IGifRepository
    {
        public Task<List<Gif>> GetTrending();
        public Task<List<Gif>> Search(string term);

    }
}
