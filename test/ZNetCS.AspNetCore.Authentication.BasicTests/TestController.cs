// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestController.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The test controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.BasicTests
{
    #region Usings

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    #endregion

    /// <summary>
    /// The test controller.
    /// </summary>
    [Authorize]
    [Route("api/test")]
    public class TestController : Controller
    {
        #region Public Methods

        /// <summary>
        /// The test get.
        /// </summary>
        [HttpGet]
        public IActionResult Get() => new OkResult();

        #endregion
    }
}