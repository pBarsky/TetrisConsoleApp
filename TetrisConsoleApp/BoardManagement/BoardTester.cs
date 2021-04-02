using System;
using TetrisConsoleApp.Bricks;

namespace TetrisConsoleApp.BoardManagement
{
    internal class BoardTester : Board
    {
        public BoardTester(int width = 10, int height = 10) : base(width, height)
        {
            PrepareTestingBoard();
        }

        private void PrepareTestingBoard()
        {
            Tab[0, 5] = 2;
            Tab[1, 5] = 2;
            Tab[2, 5] = 2;
            Tab[3, 5] = 2;
            Tab[4, 5] = 2;
        }

        private bool BrickCollisionTestcase1()
        {
            /*
             * Should always pass. Brick is placed correctly in the left corner.
             */
            BeamBrick beamBrick = new BeamBrick();
            return !IsColliding(beamBrick, 0, 0);
        }

        private bool BrickCollisionTestcase2()
        {
            /*
             * Should always fail. Brick is placed way beyond the board.
             */
            BeamBrick beamBrick = new BeamBrick(x: 100, y: 100);
            return IsColliding(beamBrick, 0, 0);
        }

        private bool BrickCollisionTestcase3()
        {
            /*
             * Brick is laid on another, already laid, brick. Should fail.
             */
            BeamBrick beamBrick = new BeamBrick(x: 2);
            return IsColliding(beamBrick, 0, 0);
        }

        public bool RunTests()
        {
            var result1 = BrickCollisionTestcase1();
            var result2 = BrickCollisionTestcase2();
            var result3 = BrickCollisionTestcase3();
            Console.WriteLine($"Testcase1 {(result1 ? "passed" : "failed")}");
            Console.WriteLine($"Testcase2 {(result2 ? "passed" : "failed")}");
            Console.WriteLine($"Testcase3 {(result3 ? "passed" : "failed")}");
            return result1 && result2 && result3;
        }
    }
}