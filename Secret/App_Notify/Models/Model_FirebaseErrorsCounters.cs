using System;
using System.Collections.Generic;
using System.Text;

namespace HIDE.RestAndRant.App_Notify.Models
{

    /// <summary>
    /// Модель счетчиков результатов Firebase
    /// </summary>
    public class Model_FirebaseErrorsCounters
    {
        /// <summary>
        /// ID с результатом PermissionDenied
        /// </summary>
        public List<int> PermissionDenied_Ids { get; set; } = [];


        /// <summary>
        /// ID с результатом InvalidArgument
        /// </summary>
        public List<int> InvalidArgument_Ids { get; set; } = [];


        /// <summary>
        /// ID с результатом Unauthenticated
        /// </summary>
        public List<int> Unauthenticated_Ids { get; set; } = [];


        /// <summary>
        /// ID с результатом InvalidError
        /// </summary>
        public List<int> InvalidError_Ids { get; set; } = [];


        /// <summary>
        /// ID с результатом NotFound
        /// </summary>
        public List<int> NotFound_Ids { get; set; } = [];


        /// <summary>
        /// ID с результатом None
        /// </summary>
        public List<int> None_Ids { get; set; } = [];


        /// <summary>
        /// ID с результатом AlreadyExists
        /// </summary>
        public List<int> AlreadyExists_Ids { get; set; } = [];

    }
}
