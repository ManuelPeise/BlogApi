using Logic.Shared.Interfaces;
using Shared.Models;

namespace Logic.Shared
{
    public abstract class ALogicBase
    {
        public ICurrentUserService _currentUserService { get; set; }

        public UserModel CurrentUser { get; private set; }
        protected ALogicBase(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            CurrentUser = _currentUserService.CurrentUser ?? throw new InvalidOperationException("Current user cannot be null.");
        }
    }
}
