# NZWalks API Implementation Plan

## Current Implementation Status

### Regions ✅
- GET /api/regions (GetAll)
- GET /api/regions/{id} (GetById)
- POST /api/regions (Create)
- PUT /api/regions/{id} (Update)
- DELETE /api/regions/{id} (Delete)

### Walks ⚠️
- GET /api/walks (GetAll)
- GET /api/walks/{id} (GetById)
- POST /api/walks (Create)
Missing:
- PUT /api/walks/{id} (Update)
- DELETE /api/walks/{id} (Delete)

### Images ⚠️
- POST /api/images/upload (Upload)
Missing:
- GET /api/images (GetAll)
- GET /api/images/{id} (GetById)
- DELETE /api/images/{id} (Delete)

### Countries ⚠️
Currently has a basic implementation with hardcoded data:
- GET /api/countries (GetAll)

Missing:
- Proper database implementation
- GET /api/countries/{id} (GetById)
- POST /api/countries (Create)
- PUT /api/countries/{id} (Update)
- DELETE /api/countries/{id} (Delete)

### Authentication ✅
- POST /api/auth/register
- POST /api/auth/login

## Implementation Plan

### 1. Complete Walks Implementation
1. Add IWalkRepository methods:
   - UpdateAsync(Guid id, Walk walk)
   - DeleteAsync(Guid id)
2. Implement these methods in SQLWalkRepository
3. Add corresponding controller actions in WalksController
4. Create UpdateWalkRequestDto

### 2. Enhance Images Implementation
1. Update IImageRepository to include:
   - GetAllAsync()
   - GetByIdAsync(Guid id)
   - DeleteAsync(Guid id)
2. Implement these methods in SQLImageRepository
3. Add corresponding controller actions in ImagesController
4. Create appropriate DTOs:
   - ImageDto for GET responses

### 3. Implement Full Countries Feature
1. Create Country domain model
2. Create CountryDto and request DTOs:
   - AddCountryRequestDto
   - UpdateCountryRequestDto
3. Add Country to NZWalksDbContext
4. Create migration for Country table
5. Create ICountryRepository interface with CRUD methods
6. Implement SQLCountryRepository
7. Update CountriesController with proper implementation
8. Add AutoMapper profiles for Country

## Step-by-Step Implementation Details

### 1. Walks Update/Delete Implementation
```csharp
// 1. Add to IWalkRepository:
Task<Walk?> UpdateAsync(Guid id, Walk walk);
Task<Walk?> DeleteAsync(Guid id);

// 2. Create UpdateWalkRequestDto
public class UpdateWalkRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string? WalkImageUrl { get; set; }
    public Guid DifficultyId { get; set; }
    public Guid RegionId { get; set; }
}
```

### 2. Images Enhancement
```csharp
// 1. Add to IImageRepository:
Task<List<Image>> GetAllAsync();
Task<Image?> GetByIdAsync(Guid id);
Task<Image?> DeleteAsync(Guid id);

// 2. Create ImageDto
public class ImageDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string? FileDescription { get; set; }
    public string FileExtension { get; set; }
    public long FileSizeInBytes { get; set; }
    public string FilePath { get; set; }
}
```

### 3. Countries Implementation
```csharp
// 1. Country Domain Model
public class Country
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string? FlagImageUrl { get; set; }
}

// 2. DTOs
public class CountryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string? FlagImageUrl { get; set; }
}

public class AddCountryRequestDto
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string? FlagImageUrl { get; set; }
}

public class UpdateCountryRequestDto
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string? FlagImageUrl { get; set; }
}
```

## Implementation Priority
1. Complete Walks CRUD (highest priority as it's a core feature)
2. Implement Countries properly (medium priority)
3. Enhance Images feature (lower priority as basic upload works)

## Notes
- Ensure proper validation is implemented for all new endpoints
- Add appropriate error handling
- Update API documentation
- Add authentication requirements for new endpoints
- Consider adding filtering and pagination for GET endpoints
