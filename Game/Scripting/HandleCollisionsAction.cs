using System;
using System.Collections.Generic;
using System.Data;
using Unit05.Game.Casting;
using Unit05.Game.Services;


namespace Unit05.Game.Scripting
{
    /// <summary>
    /// <para>An update action that handles interactions between the actors.</para>
    /// <para>
    /// The responsibility of HandleCollisionsAction is to handle the situation when the snake 
    /// collides with the food, or the snake collides with its segments, or the game is over.
    /// </para>
    /// </summary>
    public class HandleCollisionsAction : Action
    {
        private bool _isGameOver = false;
        private int _winner = 0;

        /// <summary>
        /// Constructs a new instance of HandleCollisionsAction.
        /// </summary>
        public HandleCollisionsAction()
        {
        }

        /// <inheritdoc/>
        public void Execute(Cast cast, Script script)
        {
            if (_isGameOver == false)
            {
                HandleIncrement(cast);
                HandleSegmentCollisions(cast);
                HandleGameOver(cast);
            }
        }

        /// <summary>
        /// Updates the score nd moves the food if the snake collides with it.
        /// </summary>
        /// <param name="cast">The cast of actors.</param>
        private void HandleIncrement(Cast cast)
        {
            Snake snake = (Snake)cast.GetFirstActor("snake");
            snake.GrowTail(1);

            Snake pookie = (Snake)cast.GetFirstActor("pookie");
            pookie.GrowTail(1);
        }

        /// <summary>
        /// Sets the game over flag if the snake collides with one of its segments.
        /// </summary>
        /// <param name="cast">The cast of actors.</param>
        private void HandleSegmentCollisions(Cast cast)
        {
            Snake snake = (Snake)cast.GetFirstActor("snake");
            Actor CycleHeadA = snake.GetHead();
            List<Actor> CycleBodyA = snake.GetBody();

            Snake pookie = (Snake)cast.GetFirstActor("pookie");
            Actor CycleHeadB = pookie.GetHead();
            List<Actor> CycleBodyB = pookie.GetBody();

            // create a "game over" message
            int x = Constants.MAX_X / 2;
            int y = Constants.MAX_Y / 2;
            Point position = new Point(x, y);

            Actor message = new Actor();
            message.SetPosition(position);
            cast.AddActor("messages", message);

            foreach (Actor segA in CycleBodyA)
            {
                if (CycleHeadB.GetPosition().Equals(segA.GetPosition())){
                    _isGameOver = true;
                    _winner = 1;
                    message.SetText("Player Red Won!");
                }
                if (CycleHeadA.GetPosition().Equals(segA.GetPosition())){
                    _isGameOver = true;
                    _winner = 2;
                    message.SetText("Player Green Won!");
                }
            }

            foreach (Actor segB in CycleBodyB)
            {
                if (CycleHeadA.GetPosition().Equals(segB.GetPosition())){
                    _isGameOver = true;
                    _winner = 2;
                    message.SetText("Player Green Won!");
                }
                if (CycleHeadB.GetPosition().Equals(segB.GetPosition())){
                    _isGameOver = true;
                    _winner = 1;
                    message.SetText("Player Red Won!");
            }
            }
        }

        private void HandleGameOver(Cast cast)
        {
            if (_isGameOver == true)
            {
                Snake snake = (Snake)cast.GetFirstActor("snake");
                List<Actor> segments = snake.GetSegments();
                Food food = (Food)cast.GetFirstActor("food");

                Snake pookie = (Snake)cast.GetFirstActor("pookie");
                List<Actor> pookieSeg = pookie.GetSegments();


                // make everything white
                if (_winner == 2){
                    foreach (Actor segment in segments)
                    {
                        segment.SetColor(Constants.WHITE);
                    }
                }

                if(_winner == 1){
                    foreach (Actor pooSeg in pookieSeg)
                    {
                        pooSeg.SetColor(Constants.WHITE);
                    }
                }

                // food.SetColor(Constants.WHITE);
                // pookie.SetColor(Constants.WHITE);
                // snake.SetColor(Constants.WHITE);
            }
            if (_isGameOver == false)
            {
                Snake cycleA = (Snake)cast.GetFirstActor("snake");
                List<Actor> segmentsA = cycleA.GetSegments();
                //Food food = (Food)cast.GetFirstActor("food");
                Snake cycleB = (Snake)cast.GetFirstActor("pookie");
                List<Actor> segmentsB = cycleB.GetSegments();
                //Food food = (Food)cast.GetFirstActor("food");

                foreach (Actor segment in segmentsA)
                {
                    segment.SetColor(Constants.RED);
                }      

                foreach (Actor segment in segmentsB)
                {
                    segment.SetColor(Constants.GREEN);
                }       
            }
        }

    }
}