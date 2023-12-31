﻿using Application.Features.Brands.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Transaction;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brands.Commands.Create;

public class CreateBrandCommand:IRequest<CreatedBrandResponse>,ITransactionalRequest,ICacheRemoverRequest,ILoggableRequest
{
    //Kullanicinin istek yaparken girecegi alan.(Name)
    public string Name { get; set; }

    public string? CacheKey => "";

    public bool BypassCache => false;
    public string? CacheGroupKey => "GetBrands";

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreatedBrandResponse>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly BrandBusinessRules _brandBusinnesRules;
        public CreateBrandCommandHandler(IBrandRepository brandRepository,IMapper mapper,BrandBusinessRules brandBusinessRules)
        {
            _brandBusinnesRules = brandBusinessRules;
            _brandRepository = brandRepository;
            _mapper = mapper;
        }
        //Brand gonderdigimiz zaman Handler devreye giriyor.Yani command geldiignde handler calisiyor.

        public async Task<CreatedBrandResponse>? Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            await _brandBusinnesRules.BrandNameCannotBeDuplicatedWhenInserted(request.Name);

            Brand brand = _mapper.Map<Brand>(request);  
            brand.Id = Guid.NewGuid();

            // var result = await _brandRepository.AddAsync(brand) //Alttaki de kabul bu da kabul;
            await _brandRepository.AddAsync(brand);

            //result i CreatedBrandResponse a cevir(map le)
            CreatedBrandResponse createdBrandResponse =_mapper.Map<CreatedBrandResponse>(brand);   
            return createdBrandResponse;

        }
    }
}
