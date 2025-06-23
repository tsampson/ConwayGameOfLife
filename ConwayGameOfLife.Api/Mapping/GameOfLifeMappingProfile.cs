using AutoMapper;
using ConwayGameOfLife.Models;
namespace ConwayGameOfLife.Api.Mapping;


public class GameOfLifeMappingProfile : Profile
{
    public GameOfLifeMappingProfile()
    {
        CreateMap<List<List<byte>>, byte[,]>()
            .ConvertUsing((src, dest, context) =>
            {
                var rows = src.Count;
                var cols = src[0].Count;
                var array = new byte[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        array[i,j] = src[i][j];
                    }
                }
                return array;
            });

        CreateMap<byte[,], List<List<byte>>>()
            .ConvertUsing((src, dest, context) =>
            {                 
                var rowSize = src.GetLength(0);
                var columnSize = src.GetLength(1);
                var list = new List<List<byte>>(rowSize);
                for (int x = 0; x < rowSize; x++)
                {
                    var rowList = new List<byte>(columnSize);
                    for (int y = 0; y < columnSize; y++)
                    {
                        rowList.Add(src[x, y]);
                    }
                    list.Add(rowList);
                }
                return list;
            });

        CreateMap<GameState, Dtos.GameStateResponse>();
    }
}
