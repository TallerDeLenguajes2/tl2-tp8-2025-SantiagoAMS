using interfaces;
namespace services;

public class AuthenticationService: IAuthenticationService
{
    private readonly IUserRepository _userRepo;
    private readonly IHttpContextAccessor _context;

    public AuthenticationService(IUserRepository repo, IHttpContextAccessor context)
    {
        _userRepo = repo;
        _context = context;
    }

    public bool HasAccessLevel(string requiredAccessLevel)
    {
        var con = _context.HttpContext;
        if (con == null)
        {
            _nullContext();
        }
        return con.Session.GetString("Rol") == requiredAccessLevel; 
    }

    public bool IsAuthenticated()
    {
        var con = _context.HttpContext;
        if (con == null)
        {
            _nullContext();
        }
        return con.Session.GetString("IsAuthenticated") == "true"; 
    }

    public bool Login(string username, string pass)
    {
        var con = _context.HttpContext;
        var user = _userRepo.GetUser(username,pass);
        if (user ==null){
            return false;
        }
        if (con == null)
        {
            _nullContext();
        }
        con.Session.SetString("IsAuthenticated","true");
        con.Session.SetString("User",user.User);
        con.Session.SetString("UserNombre",user.Nombre);
        con.Session.SetString("Rol",user.Rol);
        return true;
    }

    public void Logout()
    {
        var con = _context.HttpContext;
        if (con == null){
            _nullContext();
        }
        /* context.Session.Remove("IsAuthenticated"); 
        context.Session.Remove("User"); 
        context.Session.Remove("UserNombre"); 
        context.Session.Remove("Rol"); 
        */ 
        con.Session.Clear();
    }

    private static void _nullContext()
    {
        throw new InvalidOperationException("HttpContext no está disponible."); 
    }
}