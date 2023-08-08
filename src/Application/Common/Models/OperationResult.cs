using System.ComponentModel.DataAnnotations;

namespace CasseroleX.Application.Common.Models;

public enum OperationResult
{
    //basic
    [Display(Name = "Operation failed")]
    FAIL = 0,
     
    [Display(Name = "Operation successful")]
    SUCCESS = 1,

    [Display(Name = "Not development")]
    NOT_DEVELOPMENT = 10,

   
    [Display(Name = "Page not found")]
    PAGE_NOTFOUND = 11,

    [Display(Name = "Server exception")]
    SERVER_EXCEPTION = 12,

    [Display(Name = "No permission")]
    NO_PERMISSION = 13,

    //login
    [Display(Name = "Login successful")]
    LOGIN_SUCCESS = 1001,

    [Display(Name = "User has logged in")]
    LOGGED_IN = 1002, 

    //arguments
    [Display(Name = "Name already exists")]
    NAME_EXISTED = 2001,

    [Display(Name = "Passed validation")]
    PASSED_VALIDATION = 2002,


}