using AutoMapper;
using Expenses.Application.Dtos;
using Expenses.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Expenses.Infrastructure.Mappers;

public class CreateExpenseProfile : Profile
{
    public CreateExpenseProfile()
    {
        CreateExpenseMapping();
    }

    private void CreateExpenseMapping()
    {
        // Mapper pour créer une dépense à partir de ExpenseRequestDto
        CreateMap<ExpenseRequestDto, Expense>()
            .ForMember(dest => dest.IdExpense, opt => opt.Ignore())  
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now)) 
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.Now)) 
            .ForMember(dest => dest.User, opt => opt.Ignore())  
            .ForMember(dest => dest.Currency, opt => opt.Ignore()); 

        // Mapper pour convertir une dépense en ExpenseResponseDto
        CreateMap<Expense, ExpenseResponseDto>()
    .ForMember(dest => dest.UserFullName,
               opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))  
    .ForMember(dest => dest.Type,
               opt => opt.MapFrom(src => src.ExpenseType.Label))  
    .ForMember(dest => dest.Currency,
               opt => opt.MapFrom(src => src.Currency.CurrencyCode));
    }
}