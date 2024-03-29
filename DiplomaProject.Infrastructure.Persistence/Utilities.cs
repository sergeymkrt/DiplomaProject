﻿using DiplomaProject.Domain.Attributes;
using DiplomaProject.Domain.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomaProject.Infrastructure.Persistence;

public static class Utilities
{
    /// <summary>
    /// Retrieves the columns for entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity we are working with.</typeparam>
    /// <param name="depth">
    /// The depth to check for columns on related entity. Defaults to 2.
    /// </param>
    /// <param name="rootEntity">
    /// The root entity. Defaults to true.
    /// </param>
    /// <returns>The column names we can use for searching.</returns>
    public static string[] GetEntityColumns<TEntity>(int depth = 2)
        => GetEntityColumns(typeof(TEntity), depth, true);

    /// <summary>
    /// Retrieves the columns for entities.
    /// </summary>
    /// <param name="entityType">The type of the entity we are querying.</param>
    /// <param name="depth">
    /// The depth to check for columns on related entity. Defaults to 2.
    /// </param>
    /// <param name="rootEntity">
    /// The root entity. Defaults to true.
    /// </param>
    /// <param name="prefix">The prefix to use. If defined, we append column names with dot notation.</param>
    /// <returns>The column names we can use for searching.</returns>
    public static string[] GetEntityColumns(Type entityType, int depth = 2, bool rootEntity = true, string prefix = null)
    {
        if (depth < 1)
        {
            return [];
        }

        var columns = entityType
            .GetTypeInfo()
            .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(p => p.CanRead /*p.CanWrite &&*/ &&
                        !Attribute.IsDefined(p, typeof(NotMappedAttribute)) &&
                        !p.Name.Equals(nameof(Entity.CreatedBy)) &&
                        !p.Name.Equals(nameof(Entity.ModifiedBy)));

        if (rootEntity)
        {
            columns = columns.Where(p => !p.PropertyType.IsSimpleType() ||
                                         Attribute.IsDefined(p, typeof(SearchColumnAttribute)));
        }
        else
        {
            columns = columns.Where(p => Attribute.IsDefined(p, typeof(SearchColumnAttribute)) &&
                                         !p.Name.EndsWith(nameof(Entity.Id)));
        }

        var cleanColumns = new List<string>();
        foreach (var propertyInfo in columns)
        {
            var columnName = propertyInfo.Name;
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                columnName = $"{prefix.Trim()}.{columnName}";
            }

            if (propertyInfo.PropertyType != typeof(bool) &&
                (propertyInfo.PropertyType.IsSimpleType() || propertyInfo.PropertyType.IsEnumType()))
            {
                cleanColumns.Add(columnName);
            }
            else
            // Entity objects
            if (propertyInfo.PropertyType.IsSubclassOfRawGeneric(typeof(Entity)))
            {
                //if (propertyInfo.PropertyType.IsSubclassOfRawGeneric(typeof(OneToOneEntity)))
                //{
                //    if (Attribute.GetCustomAttribute(propertyInfo, typeof(NonUpdatedDataMemberAttribute)) is NonUpdatedDataMemberAttribute attribute)
                //    {
                //        continue;
                //    }
                //    cleanColumns.AddRange(GetEntityColumns(propertyInfo.PropertyType, depth, true, columnName));
                //}
                //else
                {
                    cleanColumns.AddRange(GetEntityColumns(propertyInfo.PropertyType, depth - 1, false, columnName));
                }
            }
        }

        return cleanColumns.ToArray();
    }
}
