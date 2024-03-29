﻿using Trading_company.Models;
namespace Trading_company.Areas.Manager.ViewModels
{
    /// <summary>
    /// Модельное представление нового менеджера и свободных руководителей
    /// </summary>
    public sealed class ManagerViewModel
    {
        /// <summary>
        /// Регистрируемый менеджер
        /// </summary>
        public ManagerModel Manager { get; set; } = new();

        /// <summary>
        /// Список доступных руководителей
        /// </summary>
        public List<ManagerModel> Leaders { get; set; } = new();
    }
}