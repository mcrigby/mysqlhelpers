using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore;

public static class MySqlDbContextExtensions
{
    public static IMutableProperty ConfigureEntityProperty(this IMutableProperty property, Action<IMutableProperty> configure = null)
    {
        configure?.Invoke(property);
        return property;
    }

    public static void AddCustomTypedClrProperties(this IMutableEntityType entity, Type customType, Action<IMutableProperty> configure = null)
    {
        foreach (var clrProperty in entity.GetCustomTypedClrProperties(customType))
            entity.AddCustomTypedClrProperty(clrProperty, configure);
    }
    public static IEnumerable<PropertyInfo> GetCustomTypedClrProperties(this IMutableEntityType entity, Type customType)
    {
        return entity.ClrType.GetProperties()
            .Where(x => x.PropertyType == customType);
    }
    public static IMutableProperty AddCustomTypedClrProperty(this IMutableEntityType entity, MemberInfo memberInfo, Action<IMutableProperty> configure = null)
    {
        var result = entity.AddProperty(memberInfo);
        result.ConfigureEntityProperty(configure);
        return result;
    }

    public static void ConfigureEntityPropertiesOfClrType(this IMutableEntityType entity, Type clrType, Action<IMutableProperty> configure = null)
    {
        if (configure == null)
            return;

        foreach (var entityProperty in entity.GetEntityPropertiesOfClrType(clrType))
            entityProperty.ConfigureEntityProperty(configure);
    }
    public static IEnumerable<IMutableProperty> GetEntityPropertiesOfClrType(this IMutableEntityType entity, Type clrType)
    {
        return entity.GetProperties()
            .Where(x => x.ClrType == clrType);
    }
}
