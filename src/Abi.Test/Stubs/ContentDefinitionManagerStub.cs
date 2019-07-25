using Microsoft.Extensions.Primitives;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.Test.Stubs
{
    class ContentDefinitionManagerStub : IContentDefinitionManager
    {
        public IChangeToken ChangeToken => throw new NotImplementedException();

        public void DeletePartDefinition(string name)
        {
            throw new NotImplementedException();
        }

        public void DeleteTypeDefinition(string name)
        {
            throw new NotImplementedException();
        }

        public ContentPartDefinition GetPartDefinition(string name)
        {
            throw new NotImplementedException();
        }

        public ContentTypeDefinition GetTypeDefinition(string name)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTypesHashAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ContentPartDefinition> ListPartDefinitions()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ContentTypeDefinition> ListTypeDefinitions()
        {
            throw new NotImplementedException();
        }

        public void StorePartDefinition(ContentPartDefinition contentPartDefinition)
        {
            throw new NotImplementedException();
        }

        public void StoreTypeDefinition(ContentTypeDefinition contentTypeDefinition)
        {
            throw new NotImplementedException();
        }
    }
}
