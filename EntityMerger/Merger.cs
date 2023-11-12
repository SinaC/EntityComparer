﻿using EntityMerger.EntityMerger;
using System.Collections;
using System.Reflection;

namespace EntityMerger;

public class Merger
{
    private MergeConfiguration Configuration { get; }

    internal Merger(MergeConfiguration configuration)
    {
        Configuration = configuration;
    }

    // for each existing
    //      if found in calculated (based on keys)
    //          if values not the same
    //              copy values TODO: additional values to copy which are not included in valueProperties
    //          merge existing.Many
    //          merge existing.One   TODO
    //          if values not the same or something modified when merging Many and One
    //              mark existing as Updated
    //      else
    //          mark existing as Deleted
    //          mark existing.Many as Deleted
    //          mark existing.One as Deleted
    //  for each calculated
    //      if not found in existing
    //          mark calculated as Inserted
    //          mark calculated.Many as Inserted
    //          mark calculated.One as Inserted
    public IEnumerable<TEntity> Merge<TEntity>(IEnumerable<TEntity> existingEntities, IEnumerable<TEntity> calculatedEntities)
        where TEntity : class
    {
        if (!Configuration.MergeEntityConfigurations.TryGetValue(typeof(TEntity), out var mergeEntityConfiguration))
            yield break; // TODO: exception

        var mergedEntities = Merge(mergeEntityConfiguration, existingEntities, calculatedEntities);
        foreach (var mergedEntity in mergedEntities)
        {
            yield return (TEntity)mergedEntity;
        }
    }

    private IEnumerable<object> Merge(MergeEntityConfiguration mergeEntityConfiguration, IEnumerable<object> existingEntities, IEnumerable<object> calculatedEntities)
    {
        // search if every existing entity is found in calculated entities -> this will detect update and delete
        foreach (var existingEntity in existingEntities)
        {
            var existingEntityFoundInCalculatedEntities = false;
            foreach (var calculatedEntity in calculatedEntities)
            {
                var areKeysEqual = AreEqualByPropertyInfos(mergeEntityConfiguration.KeyProperties, existingEntity, calculatedEntity);

                // existing entity found in calculated entities -> if values are different it's an update
                if (areKeysEqual)
                {
                    var areCalculatedValuesEquals = AreEqualByPropertyInfos(mergeEntityConfiguration.CalculatedValueProperties, existingEntity, calculatedEntity);
                    if (!areCalculatedValuesEquals) // calculated values are different -> copy calculated values
                        CopyValuesFromCalculatedToExistingByPropertyInfos(mergeEntityConfiguration.CalculatedValueProperties, existingEntity, calculatedEntity);

                    var mergeModificationsFound = MergeUsingNavigation(mergeEntityConfiguration, existingEntity, calculatedEntity);

                    if (!areCalculatedValuesEquals || mergeModificationsFound)
                    {
                        MarkEntity(mergeEntityConfiguration, existingEntity, EntityMergeOperation.Update);
                        yield return existingEntity;
                    }

                    existingEntityFoundInCalculatedEntities = true;
                    break; // don't need to check other calculated entities
                }
            }
            // existing entity not found in calculated entities -> it's a delete
            if (!existingEntityFoundInCalculatedEntities)
            {
                MarkEntityAndPropagateUsingNavigation(mergeEntityConfiguration, existingEntity, EntityMergeOperation.Delete); // once an entity is deleted, it's children will also be deleted
                yield return existingEntity;
            }
        }

        // search if every calculated entity is found in existing entities -> this will detect insert
        foreach (var calculatedEntity in calculatedEntities)
        {
            var calculatedEntityFoundInExistingEntities = false;
            foreach (var existingEntity in existingEntities)
            {
                var areKeysEqual = AreEqualByPropertyInfos(mergeEntityConfiguration.KeyProperties, existingEntity, calculatedEntity);
                if (areKeysEqual)
                {
                    calculatedEntityFoundInExistingEntities = true;
                    break; // don't need to check other existing entities
                }
            }
            // calculated entity not found in existing entity -> it's an insert
            if (!calculatedEntityFoundInExistingEntities)
            {
                MarkEntityAndPropagateUsingNavigation(mergeEntityConfiguration, calculatedEntity, EntityMergeOperation.Insert); // once an entity is inserted, it's children will also be inserted
                yield return calculatedEntity;
            }
        }
    }

    private void MarkEntity(MergeEntityConfiguration mergeEntityConfiguration, object entity, EntityMergeOperation operation)
    {
        var assignValue = mergeEntityConfiguration.AssignValueByOperation[operation];
        if (assignValue != null)
            assignValue.DestinationProperty.SetValue(entity, assignValue.Value);
    }

    private void MarkEntityAndPropagateUsingNavigation(MergeEntityConfiguration mergeEntityConfiguration, object entity, EntityMergeOperation operation)
    {
        MarkEntity(mergeEntityConfiguration, entity, operation);
        PropagateUsingNavigation(mergeEntityConfiguration, entity, operation);
    }

    private bool MergeUsingNavigation(MergeEntityConfiguration mergeEntityConfiguration, object existingEntity, object calculatedEntity)
    {
        var modificationsDetected = false;
        if (mergeEntityConfiguration.NavigationManyProperties != null)
        {
            foreach (var navigationManyProperty in mergeEntityConfiguration.NavigationManyProperties)
                modificationsDetected |= MergeUsingNavigationMany(navigationManyProperty, existingEntity, calculatedEntity);
        }
        if (mergeEntityConfiguration.NavigationOneProperties != null)
        {
            foreach (var navigationOneProperty in mergeEntityConfiguration.NavigationOneProperties)
                modificationsDetected |= MergeUsingNavigationOne(navigationOneProperty, existingEntity, calculatedEntity);
        }
        return modificationsDetected;
    }

    private bool MergeUsingNavigationMany(PropertyInfo navigationProperty, object existingEntity, object calculatedEntity)
    {
        if (navigationProperty == null)
            return false;
        var childType = GetNavigationManyDestinationType(navigationProperty);
        if (childType == null)
            return false;

        if (!Configuration.MergeEntityConfigurations.TryGetValue(childType, out var childMergeEntityConfiguration))
            return false; // TODO: exception

        var existingEntityChildren = navigationProperty.GetValue(existingEntity);
        var calculatedEntityChildren = navigationProperty.GetValue(calculatedEntity);

        // merge children
        var mergedChildren = Merge(childMergeEntityConfiguration, (IEnumerable<object>)existingEntityChildren, (IEnumerable<object>)calculatedEntityChildren); // TODO: remove warning

        // convert children from List<object> to List<EnityType>
        var listType = typeof(List<>).MakeGenericType(childType);
        var list = (IList)Activator.CreateInstance(listType); // TODO: remove warning
        foreach (var mergedChild in mergedChildren)
            list.Add(mergedChild);
        if (list.Count > 0)
        {
            navigationProperty.SetValue(existingEntity, list);
            return true;
        }
        return false;
    }

    private bool MergeUsingNavigationOne(PropertyInfo navigationProperty, object existingEntity, object calculatedEntity)
    {
        if (navigationProperty == null)
            return false;
        var childType = navigationProperty.PropertyType;
        if (childType == null)
            return false;

        if (!Configuration.MergeEntityConfigurations.TryGetValue(childType, out var childMergeEntityConfiguration))
            return false; // TODO: exception

        var existingEntityChild = navigationProperty.GetValue(existingEntity);
        var calculatedEntityChild = navigationProperty.GetValue(calculatedEntity);

        // was not existing and is now calculated -> it's an insert
        if (existingEntityChild == null && calculatedEntityChild != null)
        {
            navigationProperty.SetValue(existingEntity, calculatedEntityChild);
            MarkEntityAndPropagateUsingNavigation(childMergeEntityConfiguration, calculatedEntityChild, EntityMergeOperation.Insert);
            return true;
        }
        // was existing and is not calculated -> it's a delete
        if (existingEntityChild != null && calculatedEntityChild == null)
        {
            MarkEntityAndPropagateUsingNavigation(childMergeEntityConfiguration, existingEntityChild, EntityMergeOperation.Delete);
            return true;
        }
        // was existing and is calculated -> maybe an update
        if (existingEntityChild != null && calculatedEntityChild != null)
        {
            var areKeysEqual = AreEqualByPropertyInfos(childMergeEntityConfiguration.KeyProperties, existingEntityChild, calculatedEntityChild);
            if (!areKeysEqual) // keys are different -> copy keys
                CopyValuesFromCalculatedToExistingByPropertyInfos(childMergeEntityConfiguration.KeyProperties, existingEntityChild, calculatedEntityChild);

            var areCalculatedValuesEquals = AreEqualByPropertyInfos(childMergeEntityConfiguration.CalculatedValueProperties, existingEntityChild, calculatedEntityChild);
            if (!areCalculatedValuesEquals) // calculated values are different -> copy calculated values
                CopyValuesFromCalculatedToExistingByPropertyInfos(childMergeEntityConfiguration.CalculatedValueProperties, existingEntityChild, calculatedEntityChild);

            var mergeModificationsFound = MergeUsingNavigation(childMergeEntityConfiguration, existingEntityChild, calculatedEntityChild);

            if (!areKeysEqual || !areCalculatedValuesEquals || mergeModificationsFound)
            {
                MarkEntity(childMergeEntityConfiguration, existingEntityChild, EntityMergeOperation.Update);
                return true;
            }
        }
        return false;
    }

    private void CopyValuesFromCalculatedToExistingByPropertyInfos(IEnumerable<PropertyInfo> propertyInfos, object existingEntity, object calculatedEntity)
    {
        foreach (var propertyInfo in propertyInfos)
        {
            var calculatedValue = propertyInfo.GetValue(calculatedEntity);
            propertyInfo.SetValue(existingEntity, calculatedValue);
        }
    }

    private void PropagateUsingNavigation(MergeEntityConfiguration mergeEntityConfiguration, object entity, EntityMergeOperation operation)
    {
        if (mergeEntityConfiguration.NavigationManyProperties != null)
        {
            foreach (var navigationManyProperty in mergeEntityConfiguration.NavigationManyProperties)
                PropagateUsingNavigationMany(navigationManyProperty, entity, operation);
        }
        if (mergeEntityConfiguration.NavigationOneProperties != null)
        {
            foreach (var navigationOneProperty in mergeEntityConfiguration.NavigationOneProperties)
                PropagateUsingNavigationOne(navigationOneProperty, entity, operation);
        }
    }

    private void PropagateUsingNavigationMany(PropertyInfo navigationProperty, object entity, EntityMergeOperation operation)
    {
        if (navigationProperty == null)
            return;
        var childrenValue = navigationProperty.GetValue(entity);
        if (childrenValue == null)
            return;
        var childType = GetNavigationManyDestinationType(navigationProperty);
        if (childType == null)
            return;
        var assignValue = Configuration.MergeEntityConfigurations[childType].AssignValueByOperation[operation];
        if (assignValue != null)
        {
            var children = (IEnumerable<object>)childrenValue;
            foreach (var child in children)
                assignValue.DestinationProperty.SetValue(child, assignValue.Value);
        }
    }

    private void PropagateUsingNavigationOne(PropertyInfo navigationProperty, object entity, EntityMergeOperation operation)
    {
        if (navigationProperty == null)
            return;
        var childValue = navigationProperty.GetValue(entity);
        if (childValue == null)
            return;
        var childType = navigationProperty.PropertyType;
        if (childType == null)
            return;
        var assignValue = Configuration.MergeEntityConfigurations[childType].AssignValueByOperation[operation];
        if (assignValue != null)
            assignValue.DestinationProperty.SetValue(childValue, assignValue.Value);
    }

    private bool AreEqualByPropertyInfos(IEnumerable<PropertyInfo> propertyInfos, object existingEntity, object calculatedEntity)
    {
        if (propertyInfos == null)
            return true;
        foreach (var propertyInfo in propertyInfos)
        {
            var existingValue = propertyInfo.GetValue(existingEntity);
            var calculatedValue = propertyInfo.GetValue(calculatedEntity);

            if (!Equals(existingValue, calculatedValue))
                return false;
        }
        return true;
    }

    private Type GetNavigationManyDestinationType(PropertyInfo navigationProperty)
    {
        Type type = navigationProperty.PropertyType;
        // check List<>
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            return type.GetGenericArguments()[0];
        }
        // check IList<>
        var interfaceTest = new Func<Type, Type>(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>) ? i.GetGenericArguments().Single() : null); // TODO: remove warning
        var innerType = interfaceTest(type);
        if (innerType != null)
            return innerType;
        foreach (var i in type.GetInterfaces())
        {
            innerType = interfaceTest(i);
            if (innerType != null)
                return innerType;
        }
        //
        return null;
    }
}
