namespace TicTacToe.Tests.helpers;

using System.Collections.Generic;
using System.Linq;

public static class GameStateDtoBuilder
{
    public static GameStateDto InitialGameStateDto()
    {
        return AGameStateDto().Build();
    }

    public static Builder AGameStateDto()
    {
        return new Builder();
    }

    public class Builder
    {
        private List<Field> _playerX;
        private List<Field> _playerO;
        private Status _status;

        public Builder()
        {
            _playerX = new List<Field>();
            _playerO = new List<Field>();
            _status = Status.OnGoing; 
        }

        public Builder AddingFieldToX(Field field)
        {
            _playerX.Add(field);
            return this;
        }

        public Builder AddingFieldToO(Field field)
        {
            _playerO.Add(field);
            return this;
        }

        public Builder WithFieldsWithX(params Field[] playerX)
        {
            _playerX = playerX.ToList();
            return this;
        }

        public Builder WithFieldsWithO(params Field[] playerO)
        {
            _playerO = playerO.ToList();
            return this;
        }

        public Builder WinningPlayerX()
        {
            _status = Status.OverXWins;
            return this;
        }

        public Builder WinningPlayerO()
        {
            _status = Status.OverOWins;
            return this;
        }

        public Builder WithNoOneWinning()
        {
            _status = Status.OverDraw;
            return this;
        }

        public GameStateDto Build()
        {
            return new GameStateDto(_playerX, _playerO, _status);
        }
    }
}