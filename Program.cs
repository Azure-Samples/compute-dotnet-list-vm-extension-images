// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Samples.Common;
using Azure.ResourceManager.Compute;

namespace ListVirtualMachineExtensionImages
{
    public class Program
    {
        /**
         * List all virtual machine extension image publishers and
         * list all virtual machine extension images published by Microsoft.OSTCExtensions, Microsoft.Azure.Extensions
         * by browsing through extension image publishers, types, and versions.
         */
        public static async Task RunSample(ArmClient client)
        {
            var subscription = await client.GetDefaultSubscriptionAsync();
            var publishers = subscription.GetVirtualMachineImagePublishers(AzureLocation.EastUS);
            Utilities.Log("US East data center: printing list of \n"
                    + "a) Publishers and\n"
                    + "b) virtual machine images published by Microsoft.OSTCExtensions and Microsoft.Azure.Extensions");
            Utilities.Log("=======================================================");
            Utilities.Log("\n");
            foreach (var publisher in publishers)
            {
                Utilities.Log("Publisher - " + publisher.Name);
                if (StringComparer.OrdinalIgnoreCase.Equals(publisher.Name, "Microsoft.OSTCExtensions") ||
                    StringComparer.OrdinalIgnoreCase.Equals(publisher.Name, "Microsoft.Azure.Extensions"))
                {
                    Utilities.Log("\n\n");
                    Utilities.Log("=======================================================");
                    Utilities.Log("Located " + publisher.Name);
                    Utilities.Log("=======================================================");
                    Utilities.Log("Printing entries as publisher/type/version");
                    var extensionImages = subscription.GetVirtualMachineExtensionImages(AzureLocation.EastUS,publisher.Name);
                    foreach(var extensionImage in extensionImages)
                    {
                        //var type = extensionImage.Get().Value.Data.GetType();
                        //Utilities.Log(type);
                        Utilities.Log(extensionImage.Data.Name);
                    }
                    Utilities.Log("\n");
                }
            }
        }
        public static async Task Main(string[] args)
        {
            try
            {
                var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
                var clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
                var tenantId = Environment.GetEnvironmentVariable("TENANT_ID");
                var subscription = Environment.GetEnvironmentVariable("SUBSCRIPTION_ID");
                ClientSecretCredential credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                ArmClient client = new ArmClient(credential, subscription);
                await RunSample(client);
            }
            catch (Exception e)
            {
                Utilities.Log(e);
            }
        }
    }
}