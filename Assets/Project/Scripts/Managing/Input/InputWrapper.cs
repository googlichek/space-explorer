using Rewired;

namespace Game.Scripts
{
    public class InputWrapper : TickBehaviour
    {
        private Player _player;

        private int _playerId = 0;

        private bool _isMoveUpPressed;
        private bool _isMoveUpReleased;
        private bool _isMoveUpHeld;
        
        private bool _isMoveDownPressed;
        private bool _isMoveDownReleased;
        private bool _isMoveDownHeld;
        
        private bool _isMoveRightPressed;
        private bool _isMoveRightReleased;
        private bool _isMoveRightHeld;
        
        private bool _isMoveLeftPressed;
        private bool _isMoveLeftReleased;
        private bool _isMoveLeftHeld;
        
        private bool _isZoomUpPressed;
        private bool _isZoomUpReleased;
        private bool _isZoomUpHeld;
        
        private bool _isZoomDownPressed;
        private bool _isZoomDownReleased;
        private bool _isZoomDownHeld;
        
        public bool IsMoveUpPressed => _isMoveUpPressed;
        public bool IsMoveUpReleased => _isMoveUpReleased;
        public bool IsMoveUpHeld => _isMoveUpHeld;

        public bool IsMoveDownPressed => _isMoveDownPressed;
        public bool IsMoveDownReleased => _isMoveDownReleased;
        public bool IsMoveDownHeld => _isMoveDownHeld;

        public bool IsMoveRightPressed => _isMoveRightPressed;
        public bool IsMoveRightReleased => _isMoveRightReleased;
        public bool IsMoveRightHeld => _isMoveRightHeld;

        public bool IsMoveLeftPressed => _isMoveLeftPressed;
        public bool IsMoveLeftReleased => _isMoveLeftReleased;
        public bool IsMoveLeftHeld => _isMoveLeftHeld;

        public bool IsZoomUpPressed => _isZoomUpPressed;
        public bool IsZoomUpReleased => _isZoomUpReleased;
        public bool IsZoomUpHeld => _isZoomUpHeld;

        public bool IsZoomDownPressed => _isZoomDownPressed;
        public bool IsZoomDownReleased => _isZoomDownReleased;
        public bool IsZoomDownHeld => _isZoomDownHeld;

        public override void Init()
        {
            base.Init();

            priority = TickPriority.High;

            _player = ReInput.players.GetPlayer(_playerId);
        }

        public override void Tick()
        {
            base.Tick();

            HandleInput();
        }

        private void HandleInput()
        {
            _isMoveUpPressed = _player.GetButtonDown(InputActions.MoveUp);
            _isMoveUpReleased = _player.GetButtonUp(InputActions.MoveUp);
            _isMoveUpHeld = _player.GetButton(InputActions.MoveUp);

            _isMoveDownPressed = _player.GetButtonDown(InputActions.MoveDown);
            _isMoveDownReleased = _player.GetButtonUp(InputActions.MoveDown);
            _isMoveDownHeld = _player.GetButton(InputActions.MoveDown);
            
            _isMoveRightPressed = _player.GetButtonDown(InputActions.MoveRight);
            _isMoveRightReleased = _player.GetButtonUp(InputActions.MoveRight);
            _isMoveRightHeld = _player.GetButton(InputActions.MoveRight);

            _isMoveLeftPressed = _player.GetButtonDown(InputActions.MoveLeft);
            _isMoveLeftReleased = _player.GetButtonUp(InputActions.MoveLeft);
            _isMoveLeftHeld = _player.GetButton(InputActions.MoveLeft);

            _isZoomUpPressed = _player.GetButtonDown(InputActions.ZoomUp);
            _isZoomUpReleased = _player.GetButtonUp(InputActions.ZoomUp);
            _isZoomUpHeld = _player.GetButton(InputActions.ZoomUp);

            _isZoomDownPressed = _player.GetButtonDown(InputActions.ZoomDown);
            _isZoomDownReleased = _player.GetButtonUp(InputActions.ZoomDown);
            _isZoomDownHeld = _player.GetButton(InputActions.ZoomDown);
        }
    }
}
