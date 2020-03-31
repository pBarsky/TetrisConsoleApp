using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsoleApp
{
    class BoardTester : Board
    {
        public BoardTester(int width = 10, int height = 10) : base(width, height)
        {
            prepareTestingBoard();
        }

        private void prepareTestingBoard()
        {
            tab[0, 5] = 2;
            tab[1, 5] = 2;
            tab[2, 5] = 2;
            tab[3, 5] = 2;
            tab[4, 5] = 2;
        }

        public bool BrickCollisionTestcase1()
        {
            /*
             * Should always pass. Brick is placed correctly in the left corner.
             */
            BeamBrick beamBrick = new BeamBrick();
            return !IsColliding(beamBrick, 0, 0);
        }
        public bool BrickCollisionTestcase2()
        {
            /*
             * Should always fail. Brick is placed way beyond the board.
             */
            BeamBrick beamBrick = new BeamBrick(x: 100, y: 100);
            return IsColliding(beamBrick, 0, 0);
        }

        public bool BrickCollisionTestcase3()
        {
            /*
             * Brick is laid on another, already laid, brick. Should fail.
             */
            BeamBrick beamBrick = new BeamBrick(x: 2);
            return IsColliding(beamBrick, 0, 0);
        }

        public bool RunTests()
        {
            bool result1 = BrickCollisionTestcase1();
            bool result2 = BrickCollisionTestcase2();
            bool result3 = BrickCollisionTestcase3();
            Console.WriteLine($"Testcase1 {(result1 ? "passed" : "failed")}");
            Console.WriteLine($"Testcase2 {(result2 ? "passed" : "failed")}");
            Console.WriteLine($"Testcase3 {(result3 ? "passed" : "failed")}");
            return result1 && result2 && result3;
        }
    }
}
