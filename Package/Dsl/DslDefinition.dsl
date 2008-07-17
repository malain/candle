<?xml version="1.0" encoding="utf-8"?>
<Dsl dslVersion="1.0.0.0" Id="5c5acb63-d99c-4e4a-9d6a-b61acb347d12" Description="" Name="Candle" DisplayName="Candle" Namespace="DSLFactory.Candle.SystemModel" ProductName="Candle" CompanyName="DSLFactory" PackageGuid="acd37819-aa0b-4ba5-a47b-ee768b93d93e" PackageNamespace="DSLFactory.Candle.SystemModel" xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel">
  <Classes>
    <DomainClass Id="5b0a3d28-eff7-4dcd-8b5c-70a38a0f03fb" Description="The root in which all other elements are embedded. Appears as a diagram." Name="CandleModel" DisplayName="Model Root" Namespace="DSLFactory.Candle.SystemModel" HasCustomConstructor="true">
      <BaseClass>
        <DomainClassMoniker Name="CandleElement" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="47e4d406-75df-43b4-94c5-9decddca8367" Description="Description for DSLFactory.Candle.SystemModel.CandleModel.Path" Name="Path" DisplayName="Path">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(DSLFactory.Candle.SystemModel.Editor.PathEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7c85acf7-9b6b-4d5a-aa9a-43076d7fac2d" Description="Description for DSLFactory.Candle.SystemModel.CandleModel.Url" Name="Url" DisplayName="Url">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="39f8c727-0015-431b-b3c8-b96315d633fc" Description="Description for DSLFactory.Candle.SystemModel.CandleModel.Version" Name="Version" DisplayName="Version">
          <Type>
            <ExternalTypeMoniker Name="VersionInfo" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f8b9132b-48a6-4586-9481-732ef92f46d9" Description="Description for DSLFactory.Candle.SystemModel.CandleModel.Strategy Template" Name="StrategyTemplate" DisplayName="Strategy Template" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="886b9df4-034c-47c8-a2b7-b8cb6108014d" Description="Description for DSLFactory.Candle.SystemModel.CandleModel.Framework" Name="DotNetFrameworkVersion" DisplayName="Framework" DefaultValue="2.0.0.0">
          <Type>
            <ExternalTypeMoniker Name="VersionInfo" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d67108f0-59c1-47ce-bbd0-3e82ff2911a7" Description="VisibilitÃ© du composant" Name="Visibility" DisplayName="Visibility" DefaultValue="Internal">
          <Type>
            <DomainEnumerationMoniker Name="Visibility" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="bc71de9b-70da-4b53-8219-bad6b2b7dafd" Description="Description for DSLFactory.Candle.SystemModel.CandleModel.Component Type" Name="ComponentType" DisplayName="Component Type" Kind="Calculated" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="ComponentType" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c1cb9523-c5ab-490a-b3dd-87ccb1f143bc" Description="Description for DSLFactory.Candle.SystemModel.CandleModel.Is Library" Name="IsLibrary" DisplayName="Is Library" DefaultValue="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9aad6b16-eccc-4919-88c4-49eb2a7d60f8" Description="Description for DSLFactory.Candle.SystemModel.CandleModel.Base Address" Name="BaseAddress" DisplayName="Base Address">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ExternalComponent" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>CandleModelHasExternalComponents.ExternalComponents</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="BinaryComponent" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ModelRootHasComponent.Component</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="SoftwareComponent" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ModelRootHasComponent.Component</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="DataLayer" />
          </Index>
          <ForwardingPath>
            <DomainPath>ModelRootHasComponent.Component/!Component</DomainPath>
          </ForwardingPath>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="d1c67784-1874-4647-a891-540171772ced" Description="Elements embedded in the model. Appear as boxes on the diagram." Name="SoftwareComponent" DisplayName="ComponentModel" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="Component" />
      </BaseClass>
      <ElementMergeDirectives>
        <ElementMergeDirective UsesCustomMerge="true">
          <Index>
            <DomainClassMoniker Name="LayerPackage" />
          </Index>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="DataLayer" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>SoftwareComponentHasLayers.Layers</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective UsesCustomMerge="true">
          <Index>
            <DomainClassMoniker Name="Layer" />
          </Index>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="3b443c54-50c8-4461-b73c-62d3e49c9194" Description="Description for DSLFactory.Candle.SystemModel.NamedElement" Name="NamedElement" DisplayName="Named Element" InheritanceModifier="Abstract" Namespace="DSLFactory.Candle.SystemModel" HasCustomConstructor="true">
      <Properties>
        <DomainProperty Id="fbb44a41-78e1-4e00-991c-41e727d649ad" Description="Name of the element" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
          <ElementNameProvider>
            <ExternalTypeMoniker Name="/DSLFactory.Candle.SystemModel.NameProvider/SpecificNameProvider" />
          </ElementNameProvider>
        </DomainProperty>
        <DomainProperty Id="39a9315f-87bf-4e32-b1b0-f1650e6b3061" Description="The comment can contents documentation's tag like 'summary' or 'remarks'" Name="Comment" DisplayName="Comment" DefaultValue="">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="57dcc631-d010-49cd-8cb9-f4a4b3aa4816" Description="Description for DSLFactory.Candle.SystemModel.BusinessLayer" Name="BusinessLayer" DisplayName="Business Layer" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="Layer" />
      </BaseClass>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Process" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>LayerHasClassImplementations.Classes</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="565eabb5-4b50-4fff-bdaf-c00c5c8f72ef" Description="Description for DSLFactory.Candle.SystemModel.DataAccessLayer" Name="DataAccessLayer" DisplayName="Data Access Layer" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="Layer" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="91e8e06c-e29e-4507-ab86-1c9c84603e37" Description="Description for DSLFactory.Candle.SystemModel.PresentationLayer" Name="PresentationLayer" DisplayName="Presentation Layer" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="Layer" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="3b66a6e3-b4fe-48a9-9971-68588d3d1230" Description="Customizable element" Name="CandleElement" DisplayName="Candle Element" InheritanceModifier="Abstract" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="NamedElement" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="5862c6a4-f2e6-4716-a182-eb62d3998337" Description="Root name  used to generate the element's name" Name="RootName" DisplayName="Root Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="DependencyProperty" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ElementHasDependencyProperties.DependencyProperties</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="debce0c7-bc10-44bc-ab32-55a6cd5bca16" Description="Description for DSLFactory.Candle.SystemModel.DataLayer" Name="DataLayer" DisplayName="Data Layer" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="SoftwareLayer" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="2928399b-fd49-4c14-bf9d-2feecbfc7e9a" Description="Description for DSLFactory.Candle.SystemModel.DataLayer.Xml Namespace" Name="XmlNamespace" DisplayName="Xml Namespace">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Package" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>DataLayerHasPackages.Packages</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="522bb61f-1b78-49f6-a70d-79822e95d6c3" Description="Description for DSLFactory.Candle.SystemModel.TypeMember" Name="TypeMember" DisplayName="Type Member" InheritanceModifier="Abstract" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="CandleElement" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="a0a329d4-74d7-4792-b843-ddf5b8232558" Description="Description for DSLFactory.Candle.SystemModel.TypeMember.Type" Name="Type" DisplayName="Type" DefaultValue="void">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(DSLFactory.Candle.SystemModel.Strategies.TypePropertyEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7c62cabd-e1db-4c3d-9217-c399956192e7" Description="Is type a collection ?" Name="IsCollection" DisplayName="Is Collection">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a440dfa7-45a0-4e32-ba89-ed25f8d2a441" Description="Description for DSLFactory.Candle.SystemModel.TypeMember.Display Name" Name="DisplayName" DisplayName="Display Name" Kind="Calculated" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="bc773236-e7a6-40e0-9c7a-d68ed5e90a7e" Description="Description for DSLFactory.Candle.SystemModel.TypeMember.Xml Name" Name="XmlName" DisplayName="Xml Name" Category="Serialization">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="bdf885f4-eaa5-4b73-a24b-b6624a5619b4" Description="Description for DSLFactory.Candle.SystemModel.Argument" Name="Argument" DisplayName="Argument" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="TypeMember" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="da18d927-bac3-4975-a223-361f4f0a98d8" Description="" Name="Direction" DisplayName="Direction" DefaultValue="In">
          <Type>
            <DomainEnumerationMoniker Name="ArgumentDirection" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="0a14d1cc-e4f6-4de1-8ad0-7f4f8a98cb08" Description="Description for DSLFactory.Candle.SystemModel.Operation" Name="Operation" DisplayName="Operation" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="TypeMember" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="c2e0dfed-0485-4bd5-9cc1-bd9fcad0233f" Description="Custom attributes without brackets" Name="CustomAttributes" DisplayName="Custom Attributes">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Argument" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>OperationHasArguments.Arguments</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="5b5952de-ba57-4a14-a31e-469a8197d5be" Description="Description for DSLFactory.Candle.SystemModel.Package" Name="Package" DisplayName="Package" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="CandleElement" />
      </BaseClass>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="DataType" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>PackageHasTypes.Types</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="35f6d4bd-f639-434e-9594-4f1e8a14d89c" Description="Description for DSLFactory.Candle.SystemModel.DataType" Name="DataType" DisplayName="Data Type" InheritanceModifier="Abstract" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="TypeMember" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="92882263-7649-4baa-8f1d-842cf24cb01f" Description="Description for DSLFactory.Candle.SystemModel.Property" Name="Property" DisplayName="Property" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="TypeMember" />
      </BaseClass>
      <CustomTypeDescriptor>
        <DomainTypeDescriptor />
      </CustomTypeDescriptor>
      <Properties>
        <DomainProperty Id="ea5772d7-9741-4664-b560-6e88c7462116" Description="True if this property is nullable" Name="Nullable" DisplayName="Nullable">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6dcd9728-b97e-4df0-8beb-fd537b27d8dc" Description="True if this is a primary key" Name="IsPrimaryKey" DisplayName="Is Primary Key" DefaultValue="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="31bed629-453c-4ee6-8112-7b714743c597" Description="Additional custom attributes" Name="CustomAttributes" DisplayName="Custom Attributes">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="feea976f-fc57-461c-8c67-214dfaf4d7d5" Description="Description for DSLFactory.Candle.SystemModel.Property.Column Name" Name="ColumnName" DisplayName="Column Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b79d18ba-3c36-4d20-bc75-9b7cf1d6995d" Description="Description for DSLFactory.Candle.SystemModel.Property.Server Type" Name="ServerType" DisplayName="Server Type">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a717e595-7c54-42ff-a509-0c91511bec02" Description="Description for DSLFactory.Candle.SystemModel.Property.Is Auto Increment" Name="IsAutoIncrement" DisplayName="Is Auto Increment">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="89fb5141-e222-428a-af8d-1f5e53ddd496" Description="Description for DSLFactory.Candle.SystemModel.ExternalComponent" Name="ExternalComponent" DisplayName="External Component" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="CandleElement" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="55f434f2-cc29-490f-8312-4adb4f117b3e" Description="Description for DSLFactory.Candle.SystemModel.ExternalComponent.Version" Name="Version" DisplayName="Version">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(DSLFactory.Candle.SystemModel.Editor.VersionTypeEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="VersionInfo" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4df80905-f60a-489f-9285-ce41742d7901" Description="Description for DSLFactory.Candle.SystemModel.ExternalComponent.Model Moniker" Name="ModelMoniker" DisplayName="Model Moniker" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="95b52452-3a04-441b-91f2-c9f8e81bae10" Description="Used for icon" Name="IsLastVersion" DisplayName="Is Last Version" Kind="Calculated" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="74e1af2e-f383-4e5f-9d41-08ad7b70d261" Description="Description for DSLFactory.Candle.SystemModel.ExternalComponent.Description" Name="Description" DisplayName="Description">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="abca9236-3d2f-4020-9ba4-378122c12351" Description="Description for DSLFactory.Candle.SystemModel.ExternalComponent.Namespace" Name="Namespace" DisplayName="Namespace">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="b132a2e2-b3c8-48a5-ad31-f5024130a27e" Description="Description for DSLFactory.Candle.SystemModel.DependencyProperty" Name="DependencyProperty" DisplayName="Strategy Custom Property Info" Namespace="DSLFactory.Candle.SystemModel">
      <Properties>
        <DomainProperty Id="6e2b8b3d-af26-41bc-b86d-825c4dd31f86" Description="Nom" Name="StrategyId" DisplayName="Strategy Id" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="567d5969-4fce-45d7-95c1-505b803d01ed" Description="Description for DSLFactory.Candle.SystemModel.DependencyProperty.Name" Name="Name" DisplayName="Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4635cba5-91db-4513-8d53-a8c1a997287b" Description="Property value" Name="Value" DisplayName="Value">
          <Type>
            <ExternalTypeMoniker Name="/DSLFactory.Candle.SystemModel.Strategies/DependencyPropertyValue" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="1a617e79-4024-4c06-ae58-907cf1c8b506" Description="Description for DSLFactory.Candle.SystemModel.ExternalPublicPort" Name="ExternalPublicPort" DisplayName="External Public Port" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="CandleElement" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="b059bcc7-844f-42bc-9c66-fc76e3f78899" Description="Description for DSLFactory.Candle.SystemModel.ExternalPublicPort.Component Port Moniker" Name="ComponentPortMoniker" DisplayName="Component Port Moniker" GetterAccessModifier="Assembly" SetterAccessModifier="Assembly" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f48c8e18-68f3-4f59-9a48-553b7e1e31e1" Description="Description for DSLFactory.Candle.SystemModel.ExternalPublicPort.Is In Gac" Name="IsInGac" DisplayName="Is In Gac">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="6b2618d0-6e2b-4a8d-ba52-efe0f1d981d8" Description="Description for DSLFactory.Candle.SystemModel.Enumeration" Name="Enumeration" DisplayName="Enumeration" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="DataType" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="5f57c3dc-9d64-4e64-afe3-9ae3f4b2c441" Description="Description for DSLFactory.Candle.SystemModel.Enumeration.Is Flag" Name="IsFlag" DisplayName="Is Flag" DefaultValue="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="19a05e14-7dcc-4c88-9762-9e414c61f0ae" Description="Description for DSLFactory.Candle.SystemModel.UIWorkflowLayer" Name="UIWorkflowLayer" DisplayName="UIWorkflow Layer" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="Layer" />
      </BaseClass>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Scenario" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>AppWorkflowLayerHasScenarios.Scenarios</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="94c88e9d-3563-49b0-84fe-572ef8f9280e" Description="Description for DSLFactory.Candle.SystemModel.Scenario" Name="Scenario" DisplayName="Scenario" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="TypeWithOperations" />
      </BaseClass>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="UIView" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ScenarioHasUIView.Views</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="736c084d-6f53-4bb2-b8b4-361e786106dc" Description="Description for DSLFactory.Candle.SystemModel.UIView" Name="UIView" DisplayName="UIView" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="CandleElement" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="d8f9d919-d472-4cec-8816-8ff82ccd0897" Description="Description for DSLFactory.Candle.SystemModel.UIView.Description" Name="Description" DisplayName="Description">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="4095c222-0aa7-44d7-abc1-5907b7b23806" Description="Description for DSLFactory.Candle.SystemModel.DotNetAssembly" Name="DotNetAssembly" DisplayName="Dot Net Assembly" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="AbstractLayer" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="fc98b496-eb0c-41a2-8310-c29f99c517b4" Description="Description for DSLFactory.Candle.SystemModel.DotNetAssembly.Full Name" Name="FullName" DisplayName="Full Name" IsUIReadOnly="true">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(DSLFactory.Candle.SystemModel.Editor.AssemblyReferenceEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="cba8216c-2981-414f-a2d8-6dbd979c6e5b" Description="Description for DSLFactory.Candle.SystemModel.DotNetAssembly.Is In Gac" Name="IsInGac" DisplayName="Is In Gac">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b4753e0d-ddee-430b-b31a-687c8af71f63" Description="Description for DSLFactory.Candle.SystemModel.DotNetAssembly.Version" Name="Version" DisplayName="Version">
          <Type>
            <ExternalTypeMoniker Name="VersionInfo" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b98b00cb-eb4b-4622-947a-fd46fe592a5b" Description="Description for DSLFactory.Candle.SystemModel.DotNetAssembly.Initial Location" Name="InitialLocation" DisplayName="Initial Location">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="37c670d6-3e04-4ba0-84f6-f68d4844f5cb" Description="Description for DSLFactory.Candle.SystemModel.DotNetAssembly.Visibility" Name="Visibility" DisplayName="Visibility">
          <Type>
            <DomainEnumerationMoniker Name="Visibility" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="d7dd6d0e-968a-4e48-b91c-6a09aba63799" Description="Description for DSLFactory.Candle.SystemModel.AbstractLayer" Name="AbstractLayer" DisplayName="Abstract Layer" InheritanceModifier="Abstract" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="CandleElement" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="6afa3f80-edc9-479b-a646-885f04317875" Description="Nom de l'assembly gÃ©nÃ©rÃ©" Name="AssemblyName" DisplayName="Assembly Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="0b77322d-0e58-472c-8d4f-8d1f3ea452be" Description="Description for DSLFactory.Candle.SystemModel.Entity" Name="Entity" DisplayName="Entity" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="DataType" />
      </BaseClass>
      <CustomTypeDescriptor>
        <DomainTypeDescriptor />
      </CustomTypeDescriptor>
      <Properties>
        <DomainProperty Id="23c651b6-373c-4582-a747-bc8d716030dd" Description="Description for DSLFactory.Candle.SystemModel.Entity.Base Type" Name="BaseType" DisplayName="Base Type">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8b77d24b-562f-48df-9fe1-dcf1ad3bc51b" Description="Description for DSLFactory.Candle.SystemModel.Entity.Is Abstract" Name="IsAbstract" DisplayName="Is Abstract">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6037528c-c222-4b7c-93ac-f7f42eaee7d0" Description="Additional custom attributes" Name="CustomAttributes" DisplayName="Custom Attributes">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0fc628d3-8ee8-4960-ab52-d8ec24266800" Description="Description for DSLFactory.Candle.SystemModel.Entity.Table Name" Name="TableName" DisplayName="Table Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f68c094e-8beb-4666-a67c-a59d7cdcecce" Description="Description for DSLFactory.Candle.SystemModel.Entity.Table Owner" Name="TableOwner" DisplayName="Table Owner">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8eccc3d6-732a-4c26-b6de-ed198f604fc3" Description="Description for DSLFactory.Candle.SystemModel.Entity.Database Type" Name="DatabaseType" DisplayName="Database Type" DefaultValue="Table">
          <Type>
            <DomainEnumerationMoniker Name="DatabaseType" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Property" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EntityHasProperties.Properties</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="d6fc8577-845b-4904-92a6-bde2e971e8c3" Description="Description for DSLFactory.Candle.SystemModel.EnumValue" Name="EnumValue" DisplayName="Enum Value" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="TypeMember" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="65970471-93cb-4459-811d-aa0fb9fbbb98" Description="Description for DSLFactory.Candle.SystemModel.EnumValue.Value" Name="Value" DisplayName="Value">
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="187918d5-6e6a-460a-869b-07ff4d04bb85" Description="Description for DSLFactory.Candle.SystemModel.EnumValue.Has Value" Name="HasValue" DisplayName="Has Value" DefaultValue="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="5e9091a3-4b61-4e54-979f-f625d92f9fa5" Description="Description for DSLFactory.Candle.SystemModel.Artifact" Name="Artifact" DisplayName="Artifact" Namespace="DSLFactory.Candle.SystemModel">
      <Properties>
        <DomainProperty Id="182a72ba-64d0-4ffd-8044-b695a841bc66" Description="Description for DSLFactory.Candle.SystemModel.Artifact.File Name" Name="FileName" DisplayName="File Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="cd6df25e-a38b-49b3-92b6-077599f27ca8" Description="Description for DSLFactory.Candle.SystemModel.Artifact.Type" Name="Type" DisplayName="Type">
          <Type>
            <DomainEnumerationMoniker Name="ArtifactType" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="2731659b-a5d9-479a-8db0-6e91c0f64875" Description="Description for DSLFactory.Candle.SystemModel.Artifact.Initial File Name" Name="InitialFileName" DisplayName="Initial File Name" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8c49f307-b5b4-4d99-8689-dea035ad22ae" Description="Description for DSLFactory.Candle.SystemModel.Artifact.Scope" Name="Scope" DisplayName="Scope" DefaultValue="Runtime">
          <Type>
            <ExternalTypeMoniker Name="ReferenceScope" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="60d5b797-e141-4896-9a5f-6631dc9d2a41" Description="Description for DSLFactory.Candle.SystemModel.Artifact.Configuration Mode" Name="ConfigurationMode" DisplayName="Configuration Mode" DefaultValue="*">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="c3cff247-3982-4cc3-843b-dce42bb76c6f" Description="Description for DSLFactory.Candle.SystemModel.BinaryComponent" Name="BinaryComponent" DisplayName="Binary Component" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="Component" />
      </BaseClass>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="DotNetAssembly" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>BinaryComponentHasAssemblies.Assemblies</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="4dbe0df1-633a-4a00-a8e3-f2db3d75923e" Description="Description for DSLFactory.Candle.SystemModel.LayerPackage" Name="LayerPackage" DisplayName="Layer Package" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="NamedElement" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="de77ba00-d400-437b-83d0-3b26f4f30cc0" Description="Description for DSLFactory.Candle.SystemModel.LayerPackage.Level" Name="Level" DisplayName="Level" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Int16" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective UsesCustomAccept="true" UsesCustomMerge="true">
          <Index>
            <DomainClassMoniker Name="Layer" />
          </Index>
        </ElementMergeDirective>
        <ElementMergeDirective UsesCustomAccept="true" UsesCustomMerge="true" AppliesToSubclasses="false">
          <Index>
            <DomainClassMoniker Name="InterfaceLayer" />
          </Index>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="876d3e65-a9dd-4214-845e-09eca82d4845" Description="Description for DSLFactory.Candle.SystemModel.ConfigurationPart" Name="ConfigurationPart" DisplayName="Configuration Part" Namespace="DSLFactory.Candle.SystemModel">
      <Properties>
        <DomainProperty Id="c95b61f9-7a5e-4803-be67-67a206c95593" Description="Specific xml configuration relative file path " Name="XmlContent" DisplayName="Xml Content" DefaultValue="">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b6e67f25-d552-49c4-8e62-4b8f2296040c" Description="Description for DSLFactory.Candle.SystemModel.ConfigurationPart.Name" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b01c1e13-3d57-4d4d-aaeb-5d4fd431bd4b" Description="Description for DSLFactory.Candle.SystemModel.ConfigurationPart.Enabled" Name="Enabled" DisplayName="Enabled" DefaultValue="true">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e0858aa9-4997-47e0-9689-5ae0c069da34" Description="Description for DSLFactory.Candle.SystemModel.ConfigurationPart.Visibility" Name="Visibility" DisplayName="Visibility" DefaultValue="Public">
          <Type>
            <DomainEnumerationMoniker Name="Visibility" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="60bf2a2f-9b7c-406c-b804-5589b5360993" Description="Description for DSLFactory.Candle.SystemModel.ForeignKey" Name="ForeignKey" DisplayName="Foreign Key" Namespace="DSLFactory.Candle.SystemModel" />
    <DomainClass Id="a5dbd35e-2c47-4fc2-9ebe-86ffdd8bb14a" Description="Description for DSLFactory.Candle.SystemModel.TypeWithOperations" Name="TypeWithOperations" DisplayName="Type With Operations" InheritanceModifier="Abstract" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="CandleElement" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="219c5215-b648-466d-987d-41d3b418ba89" Description="Custom attributes without brackets" Name="CustomAttributes" DisplayName="Custom Attributes">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Operation" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>TypeWithOperationsHasOperations.Operations</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="26edb2d3-d12e-4eda-95de-d462f73756a5" Description="Description for DSLFactory.Candle.SystemModel.ServiceContract" Name="ServiceContract" DisplayName="Service Contract" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="TypeWithOperations" />
      </BaseClass>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Operation" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>TypeWithOperationsHasOperations.Operations</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="99fabf5f-7e7f-46d1-ac38-32b4f2c7623d" Description="Description for DSLFactory.Candle.SystemModel.InterfaceLayer" Name="InterfaceLayer" DisplayName="Interface Layer" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="SoftwareLayer" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="c92ea98d-df92-411e-8da6-ae19cd8162d6" Description="Layers's level" Name="Level" DisplayName="Level" IsBrowsable="false" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/Int16" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ServiceContract" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>InterfaceLayerHasContracts.ServiceContracts</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="c12d49a3-82f5-4bf2-bc4f-27e2f5131682" Description="Description for DSLFactory.Candle.SystemModel.Component" Name="Component" DisplayName="Component" InheritanceModifier="Abstract" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="CandleElement" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="53094dce-ee31-4598-abe5-b77f4de53cd4" Description="Description for DSLFactory.Candle.SystemModel.Component.Namespace" Name="Namespace" DisplayName="Namespace">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="581a9ebb-1f42-4bac-bf67-4af4f8a4bf00" Description="Description for DSLFactory.Candle.SystemModel.Layer" Name="Layer" DisplayName="Layer" InheritanceModifier="Abstract" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="SoftwareLayer" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="1924e6df-1cf3-4dc6-b0d9-9d6f91cc9c44" Description="Is the component's main layer" Name="HostingContext" DisplayName="Hosting Context" DefaultValue="None">
          <Type>
            <DomainEnumerationMoniker Name="HostingContext" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8a565775-a0a4-4e39-a105-97ea412b5096" Description="Is it the startup project" Name="StartupProject" DisplayName="Startup Project">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective AppliesToSubclasses="false">
          <Index>
            <DomainClassMoniker Name="ClassImplementation" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>LayerHasClassImplementations.Classes</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="ab5f4b45-871d-4e0d-9d9b-7e0f1f73f2e1" Description="Description for DSLFactory.Candle.SystemModel.SoftwareLayer" Name="SoftwareLayer" DisplayName="Software Layer" InheritanceModifier="Abstract" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="AbstractLayer" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="839442d7-7907-42a2-a06a-401ec87ed7c3" Description="Description for DSLFactory.Candle.SystemModel.SoftwareLayer.Template" Name="Template" DisplayName="Template">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(DSLFactory.Candle.SystemModel.Editor.VSTemplateTypeEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f6e10985-013f-4000-8e0c-2ce18c24375f" Description="Description for DSLFactory.Candle.SystemModel.SoftwareLayer.Namespace" Name="Namespace" DisplayName="Namespace">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="19609ee8-1db0-46c6-ba77-a19b36acf7ab" Description="Visual Studio Project Name" Name="VSProjectName" DisplayName="VSProject Name" Kind="CustomStorage">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="SoftwareLayer" />
          </Index>
          <ForwardingPath>
            <DomainPath>SoftwareComponentHasLayers.Component/!SoftwareComponent</DomainPath>
          </ForwardingPath>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="298db53b-6dbb-428c-8652-c37dd95b0832" Description="Description for DSLFactory.Candle.SystemModel.ClassImplementation" Name="ClassImplementation" DisplayName="Class Implementation" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="TypeWithOperations" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="092e4ff2-02fb-4f74-a445-18c72bed78fc" Description="Description for DSLFactory.Candle.SystemModel.Process" Name="Process" DisplayName="Process" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="ClassImplementation" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="2f2eb3c3-bbab-4893-997d-2d61597c89bd" Description="Description for DSLFactory.Candle.SystemModel.ExternalServiceContract" Name="ExternalServiceContract" DisplayName="External Service Contract" Namespace="DSLFactory.Candle.SystemModel">
      <BaseClass>
        <DomainClassMoniker Name="ExternalPublicPort" />
      </BaseClass>
    </DomainClass>
  </Classes>
  <Relationships>
    <DomainRelationship Id="e2dec53b-ff9e-40c6-b5f9-d88aa211920e" Description="Description for DSLFactory.Candle.SystemModel.OperationHasArguments" Name="OperationHasArguments" DisplayName="Operation Has Arguments" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="bd4c0b80-ae02-478f-addb-6bd49d1143c9" Description="Description for DSLFactory.Candle.SystemModel.OperationHasArguments.Operation" Name="Operation" DisplayName="Operation" PropertyName="Arguments" PropertyDisplayName="Arguments">
          <RolePlayer>
            <DomainClassMoniker Name="Operation" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="209590a7-9846-47ef-89ea-502dba66ea58" Description="Description for DSLFactory.Candle.SystemModel.OperationHasArguments.Argument" Name="Argument" DisplayName="Argument" PropertyName="Operation" Multiplicity="ZeroOne" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Operation">
          <RolePlayer>
            <DomainClassMoniker Name="Argument" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="c265691f-17f8-426f-8cc3-44b9450ec544" Description="Description for DSLFactory.Candle.SystemModel.PackageHasTypes" Name="PackageHasTypes" DisplayName="Package Has Types" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="9b148f6f-b75e-41ae-ab1e-714f0989f631" Description="Description for DSLFactory.Candle.SystemModel.PackageHasTypes.Package" Name="Package" DisplayName="Package" PropertyName="Types" PropertyDisplayName="Types">
          <RolePlayer>
            <DomainClassMoniker Name="Package" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="5894c39a-60f6-4d7f-8fdb-fbc5e2261eb7" Description="Description for DSLFactory.Candle.SystemModel.PackageHasTypes.TypeModel" Name="TypeModel" DisplayName="Type Model" PropertyName="Package" Multiplicity="ZeroOne" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Package">
          <RolePlayer>
            <DomainClassMoniker Name="DataType" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="a0d65794-8063-4219-85e5-c949729b88d4" Description="Description for DSLFactory.Candle.SystemModel.DataLayerHasPackages" Name="DataLayerHasPackages" DisplayName="Data Layer Has Packages" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="a3c591eb-8fb1-49ae-b5c2-ed652c5bda92" Description="Description for DSLFactory.Candle.SystemModel.DataLayerHasPackages.Layer" Name="Layer" DisplayName="Layer" PropertyName="Packages" PropertyDisplayName="Packages">
          <RolePlayer>
            <DomainClassMoniker Name="DataLayer" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="4ac7c1c4-77fe-4a0c-bcc1-7f4ece3ccefa" Description="Description for DSLFactory.Candle.SystemModel.DataLayerHasPackages.Package" Name="Package" DisplayName="Package" PropertyName="Layer" Multiplicity="ZeroOne" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Layer">
          <RolePlayer>
            <DomainClassMoniker Name="Package" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="b54f35dc-10d9-44cc-b8d6-6a3020300a89" Description="Description for DSLFactory.Candle.SystemModel.Association" Name="Association" DisplayName="Association" Namespace="DSLFactory.Candle.SystemModel" AllowsDuplicates="true">
      <CustomTypeDescriptor>
        <DomainTypeDescriptor />
      </CustomTypeDescriptor>
      <Properties>
        <DomainProperty Id="504925ae-0856-4396-96cf-bbe388cc9908" Description="Description for DSLFactory.Candle.SystemModel.Association.Xml Name" Name="XmlName" DisplayName="Xml Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="91c3ea75-25f1-4cab-a061-6790c739b1bc" Description="Description for DSLFactory.Candle.SystemModel.Association.Source Role Name" Name="SourceRoleName" DisplayName="Source Role Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7f374a2b-5059-4ac5-ab7b-36bf41503c54" Description="Description for DSLFactory.Candle.SystemModel.Association.Target Role Name" Name="TargetRoleName" DisplayName="Target Role Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="fd2462b9-d5b0-4271-a641-bb474aae61b5" Description="Description for DSLFactory.Candle.SystemModel.Association.Target Multiplicity" Name="TargetMultiplicity" DisplayName="Target Multiplicity" DefaultValue="NotApplicable">
          <Type>
            <DomainEnumerationMoniker Name="Multiplicity" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="1fadb2a6-7636-441d-9fe0-ecc49b30d89e" Description="Description for DSLFactory.Candle.SystemModel.Association.Source Multiplicity" Name="SourceMultiplicity" DisplayName="Source Multiplicity" DefaultValue="ZeroOne">
          <Type>
            <DomainEnumerationMoniker Name="Multiplicity" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="63a7f5e3-82a7-4f67-86e7-b1803807a710" Description="Description for DSLFactory.Candle.SystemModel.Association.Sort" Name="Sort" DisplayName="Sort" DefaultValue="Normal">
          <Type>
            <DomainEnumerationMoniker Name="AssociationSort" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ForeignKey" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>AssociationHasForeignKeys.ForeignKeys</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
      <Source>
        <DomainRole Id="7ef05dfa-9ef0-4e18-91fa-1e419a5802dd" Description="Description for DSLFactory.Candle.SystemModel.Association.Source" Name="Source" DisplayName="Source" PropertyName="Targets" PropertyDisplayName="Targets">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="19117517-5f57-446c-9824-7a19c275a9d9" Description="Description for DSLFactory.Candle.SystemModel.Association.Target" Name="Target" DisplayName="Target" PropertyName="Sources" PropertyDisplayName="Sources">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="8f820084-65fd-4254-af7c-ede926f5accc" Description="Description for DSLFactory.Candle.SystemModel.CandleModelHasExternalComponents" Name="CandleModelHasExternalComponents" DisplayName="Model Root Has External Components" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="4edd11d4-d015-4965-8d44-293a8f0e6dd3" Description="Description for DSLFactory.Candle.SystemModel.CandleModelHasExternalComponents.CandleModel" Name="CandleModel" DisplayName="Model Root" PropertyName="ExternalComponents" PropertyDisplayName="External Components">
          <RolePlayer>
            <DomainClassMoniker Name="CandleModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="d9375911-7bd7-49f4-873f-03b690657618" Description="Description for DSLFactory.Candle.SystemModel.CandleModelHasExternalComponents.ExternalComponent" Name="ExternalComponent" DisplayName="External Component" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Model">
          <RolePlayer>
            <DomainClassMoniker Name="ExternalComponent" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="9f2f9051-7eb9-4641-9b8a-1d21e7211d0a" Description="Description for DSLFactory.Candle.SystemModel.ScenarioHasUIView" Name="ScenarioHasUIView" DisplayName="Scenario Has UIView" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="2636fc47-6960-4c58-ba59-ba1d691363be" Description="Description for DSLFactory.Candle.SystemModel.ScenarioHasUIView.Scenario" Name="Scenario" DisplayName="Scenario" PropertyName="Views" PropertyDisplayName="Views">
          <RolePlayer>
            <DomainClassMoniker Name="Scenario" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="99684c50-b093-4b04-9c53-bd3b1007af66" Description="Description for DSLFactory.Candle.SystemModel.ScenarioHasUIView.View" Name="View" DisplayName="View" PropertyName="Scenario" Multiplicity="ZeroOne" PropagatesDelete="true" PropagatesCopy="true" IsPropertyBrowsable="false" PropertyDisplayName="Scenario">
          <RolePlayer>
            <DomainClassMoniker Name="UIView" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="6fc6bb31-9db7-485e-bfae-ebe90cdfd3ff" Description="Description for DSLFactory.Candle.SystemModel.AppWorkflowLayerHasScenarios" Name="AppWorkflowLayerHasScenarios" DisplayName="App Workflow Layer Has Scenarios" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="811fc3f8-d1d4-4866-bd99-ffd2906eed05" Description="Description for DSLFactory.Candle.SystemModel.AppWorkflowLayerHasScenarios.Layer" Name="Layer" DisplayName="Layer" PropertyName="Scenarios" PropertyDisplayName="Scenarios">
          <RolePlayer>
            <DomainClassMoniker Name="UIWorkflowLayer" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="28d40e4d-ce39-4b4f-bf3c-e1c5d5ba3f1a" Description="Description for DSLFactory.Candle.SystemModel.AppWorkflowLayerHasScenarios.Scenario" Name="Scenario" DisplayName="Scenario" PropertyName="Layer" Multiplicity="ZeroOne" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Layer">
          <RolePlayer>
            <DomainClassMoniker Name="Scenario" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="a33b9e7e-c6a2-45fc-ba47-bf174c9ef2d1" Description="Description for DSLFactory.Candle.SystemModel.Action" Name="Action" DisplayName="Action" Namespace="DSLFactory.Candle.SystemModel" AllowsDuplicates="true">
      <Properties>
        <DomainProperty Id="20c9b69c-8e40-4c85-87d2-0ea60ec7d745" Description="Description for DSLFactory.Candle.SystemModel.Action.Name" Name="Name" DisplayName="Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b8cf6ca2-fc2f-4430-ae11-1f382cd99af9" Description="Description for DSLFactory.Candle.SystemModel.Action.Roles" Name="Roles" DisplayName="Roles">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9ceda67b-0fd0-4a8b-99ae-75b39c41b43f" Description="Description for DSLFactory.Candle.SystemModel.Action.Description" Name="Description" DisplayName="Description">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="a853cb90-f304-437f-b53e-9f42b88ff0fb" Description="Description for DSLFactory.Candle.SystemModel.Action.ViewSource" Name="ViewSource" DisplayName="View Source" PropertyName="ViewTargets" PropertyDisplayName="View Targets">
          <RolePlayer>
            <DomainClassMoniker Name="UIView" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="8af0718d-0c5d-4368-bf49-3e2f57634c2d" Description="Description for DSLFactory.Candle.SystemModel.Action.ViewTarget" Name="ViewTarget" DisplayName="View Target" PropertyName="ViewSources" PropertyDisplayName="View Sources">
          <RolePlayer>
            <DomainClassMoniker Name="UIView" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="cc67e310-ab02-4772-98c0-5db45ee7c073" Description="Description for DSLFactory.Candle.SystemModel.EnumHasValues" Name="EnumHasValues" DisplayName="Enum Has Values" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="639b9aaa-d17b-4ecd-a931-c659feefe1a5" Description="Description for DSLFactory.Candle.SystemModel.EnumHasValues.Parent" Name="Parent" DisplayName="Parent" PropertyName="Values" PropertyDisplayName="Values">
          <RolePlayer>
            <DomainClassMoniker Name="Enumeration" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="f84ffb75-e706-4051-bc18-c5d6263e6645" Description="Description for DSLFactory.Candle.SystemModel.EnumHasValues.Value" Name="Value" DisplayName="Value" PropertyName="Parent" Multiplicity="ZeroOne" PropertyDisplayName="Parent">
          <RolePlayer>
            <DomainClassMoniker Name="EnumValue" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="afb45649-7753-4a8f-8d3b-4694750af0d3" Description="Description for DSLFactory.Candle.SystemModel.EntityHasProperties" Name="EntityHasProperties" DisplayName="Entity Has Properties" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="b70fa7b8-5310-47e3-9ca2-c165ddf85029" Description="Description for DSLFactory.Candle.SystemModel.EntityHasProperties.Parent" Name="Parent" DisplayName="Parent" PropertyName="Properties" PropertyDisplayName="Properties">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="0d979f17-f00e-4e90-a529-e9c86f9c3362" Description="Description for DSLFactory.Candle.SystemModel.EntityHasProperties.Property" Name="Property" DisplayName="Property" PropertyName="Parent" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Parent">
          <RolePlayer>
            <DomainClassMoniker Name="Property" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="e1439439-e8be-45ce-a6da-fd6be3f55a7e" Description="Description for DSLFactory.Candle.SystemModel.ElementHasDependencyProperties" Name="ElementHasDependencyProperties" DisplayName="Element Has Dependency Properties" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="9376207c-6915-4b2d-a319-0871ac5e42cd" Description="Description for DSLFactory.Candle.SystemModel.ElementHasDependencyProperties.Parent" Name="Parent" DisplayName="Parent" PropertyName="DependencyProperties" PropertyDisplayName="Dependency Properties">
          <RolePlayer>
            <DomainClassMoniker Name="CandleElement" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="edf83250-044d-4802-90e0-afed6701c4e0" Description="Description for DSLFactory.Candle.SystemModel.ElementHasDependencyProperties.DependencyProperty" Name="DependencyProperty" DisplayName="Dependency Property" PropertyName="Parent" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" PropagatesCopy="true" IsPropertyBrowsable="false" PropertyDisplayName="Parent">
          <RolePlayer>
            <DomainClassMoniker Name="DependencyProperty" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="ee57587f-2835-4c81-9488-57a6bfebd116" Description="Description for DSLFactory.Candle.SystemModel.AssemblyReferencesAssemblies" Name="AssemblyReferencesAssemblies" DisplayName="Assembly References Assemblies" Namespace="DSLFactory.Candle.SystemModel">
      <Properties>
        <DomainProperty Id="9f58fa5a-2339-43e4-a1a2-87a9b5465841" Description="Description for DSLFactory.Candle.SystemModel.AssemblyReferencesAssemblies.Scope" Name="Scope" DisplayName="Scope">
          <Type>
            <ExternalTypeMoniker Name="ReferenceScope" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="409aaf12-f549-4ab7-8ba4-08c0eb1992eb" Description="Description for DSLFactory.Candle.SystemModel.AssemblyReferencesAssemblies.TargetAssembly" Name="TargetAssembly" DisplayName="Target Assembly" PropertyName="InternalAssemblyReferences" PropertyDisplayName="Internal Assembly References">
          <RolePlayer>
            <DomainClassMoniker Name="DotNetAssembly" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="6a01436f-eb71-47d0-a0af-482ce6a590a3" Description="Description for DSLFactory.Candle.SystemModel.AssemblyReferencesAssemblies.SourceAssembly" Name="SourceAssembly" DisplayName="Source Assembly" PropertyName="ReferencedByAssemblies" IsPropertyGenerator="false" PropertyDisplayName="Referenced By Assemblies">
          <RolePlayer>
            <DomainClassMoniker Name="DotNetAssembly" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="9b32bb29-0cea-4bf4-b942-21fdbfefae1b" Description="Description for DSLFactory.Candle.SystemModel.AssociationHasProperties" Name="AssociationHasProperties" DisplayName="Association Has Properties" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="f0431984-ae48-415d-9d10-f72d8c0f67ac" Description="Description for DSLFactory.Candle.SystemModel.AssociationHasProperties.Association" Name="Association" DisplayName="Association" PropertyName="DependencyProperties" PropertyDisplayName="Dependency Properties">
          <RolePlayer>
            <DomainRelationshipMoniker Name="Association" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="4bc0725b-10e2-49e5-8603-622bbf68815a" Description="Description for DSLFactory.Candle.SystemModel.AssociationHasProperties.DependencyProperty" Name="DependencyProperty" DisplayName="Dependency Property" PropertyName="Association" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" PropagatesCopy="true" IsPropertyBrowsable="false" PropertyDisplayName="Association">
          <RolePlayer>
            <DomainClassMoniker Name="DependencyProperty" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="48086dd7-1a10-4ce7-a7b5-6f6ee4b512ce" Description="Description for DSLFactory.Candle.SystemModel.ActionHasDependencyProperties" Name="ActionHasDependencyProperties" DisplayName="Action Has Dependency Properties" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="953238e2-e8f0-40a8-b193-9acebdf0f0fb" Description="Description for DSLFactory.Candle.SystemModel.ActionHasDependencyProperties.Action" Name="Action" DisplayName="Action" PropertyName="DependencyProperties" PropertyDisplayName="Dependency Properties">
          <RolePlayer>
            <DomainRelationshipMoniker Name="Action" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="16d3cf91-57fb-49e6-8a0b-4117a425fcef" Description="Description for DSLFactory.Candle.SystemModel.ActionHasDependencyProperties.DependencyProperty" Name="DependencyProperty" DisplayName="Dependency Property" PropertyName="Action" Multiplicity="ZeroOne" IsPropertyGenerator="false" PropertyDisplayName="Action">
          <RolePlayer>
            <DomainClassMoniker Name="DependencyProperty" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="4cebbeaa-bf86-4ff3-a7ac-ecb7d994e80f" Description="Description for DSLFactory.Candle.SystemModel.AssociationHasForeignKeys" Name="AssociationHasForeignKeys" DisplayName="Association Has Foreign Keys" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="30c7eb59-0376-4654-8afe-6e76353e3e40" Description="Description for DSLFactory.Candle.SystemModel.AssociationHasForeignKeys.Association" Name="Association" DisplayName="Association" PropertyName="ForeignKeys" PropertyDisplayName="Foreign Keys">
          <RolePlayer>
            <DomainRelationshipMoniker Name="Association" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="11f9ae57-8fd0-41bf-a04b-bb9b66a0f35b" Description="Description for DSLFactory.Candle.SystemModel.AssociationHasForeignKeys.ForeignKey" Name="ForeignKey" DisplayName="Foreign Key" PropertyName="Association" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Association">
          <RolePlayer>
            <DomainClassMoniker Name="ForeignKey" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="33738785-17a6-4ed7-a604-3ffa48a87c7e" Description="Description for DSLFactory.Candle.SystemModel.ForeignKeyReferencesProperty" Name="ForeignKeyReferencesProperty" DisplayName="Foreign Key References Property" Namespace="DSLFactory.Candle.SystemModel">
      <Source>
        <DomainRole Id="55f9e3d6-cc51-4cf3-ad24-7ceb36362a85" Description="Description for DSLFactory.Candle.SystemModel.ForeignKeyReferencesProperty.ForeignKey" Name="ForeignKey" DisplayName="Foreign Key" PropertyName="Column" Multiplicity="One" PropertyDisplayName="Column">
          <RolePlayer>
            <DomainClassMoniker Name="ForeignKey" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="63387a3a-aef7-4285-b104-9c05eece0b9c" Description="Description for DSLFactory.Candle.SystemModel.ForeignKeyReferencesProperty.Property" Name="Property" DisplayName="Property" PropertyName="ForeignKeys" IsPropertyGenerator="false" PropertyDisplayName="Foreign Keys">
          <RolePlayer>
            <DomainClassMoniker Name="Property" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="f0238ad9-0d54-4338-a97e-892a7b6b616b" Description="Description for DSLFactory.Candle.SystemModel.ForeignKeyReferencesPrimaryKey" Name="ForeignKeyReferencesPrimaryKey" DisplayName="Foreign Key References Primary Key" Namespace="DSLFactory.Candle.SystemModel">
      <Source>
        <DomainRole Id="e82680e9-4192-493e-91dd-e1b4421c0f19" Description="Description for DSLFactory.Candle.SystemModel.ForeignKeyReferencesPrimaryKey.ForeignKey" Name="ForeignKey" DisplayName="Foreign Key" PropertyName="PrimaryKey" Multiplicity="One" PropertyDisplayName="Primary Key">
          <RolePlayer>
            <DomainClassMoniker Name="ForeignKey" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="80590438-39bb-4f15-8ffd-9be605dcdb91" Description="Description for DSLFactory.Candle.SystemModel.ForeignKeyReferencesPrimaryKey.Property" Name="Property" DisplayName="Property" PropertyName="ForeignKeys" IsPropertyGenerator="false" PropertyDisplayName="Foreign Keys">
          <RolePlayer>
            <DomainClassMoniker Name="Property" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="76cb69a5-0d71-4723-9f2c-429aa33eae64" Description="Description for DSLFactory.Candle.SystemModel.TypeWithOperationsHasOperations" Name="TypeWithOperationsHasOperations" DisplayName="Type With Operations Has Operations" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="206d7a4e-17a9-4c1d-9165-fbd5618e36fb" Description="Description for DSLFactory.Candle.SystemModel.TypeWithOperationsHasOperations.Parent" Name="Parent" DisplayName="Parent" PropertyName="Operations" PropertyDisplayName="Operations">
          <RolePlayer>
            <DomainClassMoniker Name="TypeWithOperations" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="de73b35f-f44f-48e9-b559-a0ffcc940d86" Description="Description for DSLFactory.Candle.SystemModel.TypeWithOperationsHasOperations.Operation" Name="Operation" DisplayName="Operation" PropertyName="Parent" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Parent">
          <RolePlayer>
            <DomainClassMoniker Name="Operation" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="f3dbc221-9d31-4e50-a9c6-dc1e7388ad23" Description="Description for DSLFactory.Candle.SystemModel.ModelRootHasComponent" Name="ModelRootHasComponent" DisplayName="Model Root Has Component" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="5d922d58-95f7-41bc-9aeb-75a96ec889e0" Description="Description for DSLFactory.Candle.SystemModel.ModelRootHasComponent.CandleModel" Name="CandleModel" DisplayName="Model Root" PropertyName="Component" Multiplicity="ZeroOne" PropertyDisplayName="Component">
          <RolePlayer>
            <DomainClassMoniker Name="CandleModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="99972ceb-e13e-43d9-b76b-228509f40b32" Description="Description for DSLFactory.Candle.SystemModel.ModelRootHasComponent.Component" Name="Component" DisplayName="Component" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Model">
          <RolePlayer>
            <DomainClassMoniker Name="Component" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="181772b6-8a28-46ee-9964-068820e0280c" Description="Description for DSLFactory.Candle.SystemModel.ExternalServiceReference" Name="ExternalServiceReference" DisplayName="External Service Reference" Namespace="DSLFactory.Candle.SystemModel">
      <Properties>
        <DomainProperty Id="073ecae8-e64f-4549-8501-e5684ec47ca7" Description="Description for DSLFactory.Candle.SystemModel.ExternalServiceReference.Configuration Mode" Name="ConfigurationMode" DisplayName="Configuration Mode" DefaultValue="*">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7723fd9c-73f2-407d-a836-a36ffb4332a3" Description="Description for DSLFactory.Candle.SystemModel.ExternalServiceReference.Scope" Name="Scope" DisplayName="Scope" DefaultValue="Compilation">
          <Type>
            <ExternalTypeMoniker Name="ReferenceScope" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="6d3a67cb-1918-4516-9fdc-66f934c5a7e2" Description="Description for DSLFactory.Candle.SystemModel.ExternalServiceReference.Client" Name="Client" DisplayName="Client" PropertyName="ExternalServiceReferences" PropertyDisplayName="External Service References">
          <RolePlayer>
            <DomainClassMoniker Name="AbstractLayer" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="66817e3f-8a01-4efb-9926-bd36a39e0fe1" Description="Description for DSLFactory.Candle.SystemModel.ExternalServiceReference.ExternalPublicPort" Name="ExternalPublicPort" DisplayName="External Public Port" PropertyName="Clients" PropertyDisplayName="Clients">
          <RolePlayer>
            <DomainClassMoniker Name="ExternalPublicPort" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="e873a742-51f0-4194-9c66-8380fc6057a6" Description="Description for DSLFactory.Candle.SystemModel.LayerHasArtifacts" Name="LayerHasArtifacts" DisplayName="Layer Has Artifacts" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="2394aa19-d589-4b0e-866f-c49e2b64b454" Description="Description for DSLFactory.Candle.SystemModel.LayerHasArtifacts.AbstractLayer" Name="AbstractLayer" DisplayName="Abstract Layer" PropertyName="Artifacts" PropertyDisplayName="Artifacts">
          <RolePlayer>
            <DomainClassMoniker Name="AbstractLayer" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="b9d8e6a1-95c0-462f-a443-62902a39ec8e" Description="Description for DSLFactory.Candle.SystemModel.LayerHasArtifacts.Artifact" Name="Artifact" DisplayName="Artifact" PropertyName="Layer" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" PropagatesCopy="true" PropertyDisplayName="Layer">
          <RolePlayer>
            <DomainClassMoniker Name="Artifact" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="b461b9ea-396d-47da-a827-2221237d7865" Description="Description for DSLFactory.Candle.SystemModel.InterfaceLayerHasContracts" Name="InterfaceLayerHasContracts" DisplayName="Interface Layer Has Contracts" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="9d12181d-bd22-4ae7-aec8-7f79ba5fd1f0" Description="Description for DSLFactory.Candle.SystemModel.InterfaceLayerHasContracts.InterfaceLayer" Name="InterfaceLayer" DisplayName="Interface Layer" PropertyName="ServiceContracts" PropertyDisplayName="Service Contracts">
          <RolePlayer>
            <DomainClassMoniker Name="InterfaceLayer" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="3d04b83e-5824-41d9-bc2a-044091a198b3" Description="Description for DSLFactory.Candle.SystemModel.InterfaceLayerHasContracts.ServiceContract" Name="ServiceContract" DisplayName="Service Contract" PropertyName="Layer" Multiplicity="ZeroOne" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Layer">
          <RolePlayer>
            <DomainClassMoniker Name="ServiceContract" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="32eed3f0-d6f4-4ffc-8835-f3fb97720e5b" Description="Description for DSLFactory.Candle.SystemModel.ExternalServiceReferenceHasDependencyProperties" Name="ExternalServiceReferenceHasDependencyProperties" DisplayName="External Service Reference Has Dependency Properties" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="bb8ebd98-9cd8-478a-b025-27beaba997e9" Description="Description for DSLFactory.Candle.SystemModel.ExternalServiceReferenceHasDependencyProperties.ExternalServiceReference" Name="ExternalServiceReference" DisplayName="External Service Reference" PropertyName="DependencyProperties" PropertyDisplayName="Dependency Properties">
          <RolePlayer>
            <DomainRelationshipMoniker Name="ExternalServiceReference" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="14e85894-41d0-472c-bafb-52c430fcfab0" Description="Description for DSLFactory.Candle.SystemModel.ExternalServiceReferenceHasDependencyProperties.DependencyProperty" Name="DependencyProperty" DisplayName="Dependency Property" PropertyName="ExternalServiceReference" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" PropagatesCopy="true" PropertyDisplayName="External Service Reference">
          <RolePlayer>
            <DomainClassMoniker Name="DependencyProperty" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="2f142ad9-cb6b-47b6-bf8f-9c2ba54d44fd" Description="Description for DSLFactory.Candle.SystemModel.LayerPackageContainsLayers" Name="LayerPackageContainsLayers" DisplayName="Layer Package Contains Layers" Namespace="DSLFactory.Candle.SystemModel">
      <Source>
        <DomainRole Id="d215b379-0279-469f-80ef-b8dcb02b738a" Description="Description for DSLFactory.Candle.SystemModel.LayerPackageContainsLayers.LayerPackage" Name="LayerPackage" DisplayName="Layer Package" PropertyName="Layers" PropertyDisplayName="Layers">
          <RolePlayer>
            <DomainClassMoniker Name="LayerPackage" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="f0b1e9ce-abc8-4ae0-97fd-f5dced513697" Description="Description for DSLFactory.Candle.SystemModel.LayerPackageContainsLayers.Layer" Name="Layer" DisplayName="Layer" PropertyName="LayerPackage" Multiplicity="ZeroOne" IsPropertyBrowsable="false" PropertyDisplayName="Layer Package">
          <RolePlayer>
            <DomainClassMoniker Name="Layer" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="ead3b962-b3f8-43e3-bc1d-2f62ceb86c27" Description="Description for DSLFactory.Candle.SystemModel.SoftwareComponentHasLayers" Name="SoftwareComponentHasLayers" DisplayName="Software Component Has Layers" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="beefa80a-ca79-4f6c-9384-7c0db35595ef" Description="Description for DSLFactory.Candle.SystemModel.SoftwareComponentHasLayers.SoftwareComponent" Name="SoftwareComponent" DisplayName="Software Component" PropertyName="Layers" PropertyDisplayName="Layers">
          <RolePlayer>
            <DomainClassMoniker Name="SoftwareComponent" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="4bf4f7ca-4ffc-495c-bed1-14e4c1c098b0" Description="Description for DSLFactory.Candle.SystemModel.SoftwareComponentHasLayers.SoftwareLayer" Name="SoftwareLayer" DisplayName="Software Layer" PropertyName="Component" Multiplicity="ZeroOne" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Component">
          <RolePlayer>
            <DomainClassMoniker Name="SoftwareLayer" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="5e8390d9-4e25-439e-921e-a17fd22b86e3" Description="Description for DSLFactory.Candle.SystemModel.BinaryComponentHasAssemblies" Name="BinaryComponentHasAssemblies" DisplayName="Binary Component Has Assemblies" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="bbd05a00-e782-4f80-aedc-61dabc3d836d" Description="Description for DSLFactory.Candle.SystemModel.BinaryComponentHasAssemblies.BinaryComponent" Name="BinaryComponent" DisplayName="Binary Component" PropertyName="Assemblies" PropertyDisplayName="Assemblies">
          <RolePlayer>
            <DomainClassMoniker Name="BinaryComponent" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="82ff52ec-b11f-4798-9807-3d9aa58b668c" Description="Description for DSLFactory.Candle.SystemModel.BinaryComponentHasAssemblies.DotNetAssembly" Name="DotNetAssembly" DisplayName="Dot Net Assembly" PropertyName="Component" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Component">
          <RolePlayer>
            <DomainClassMoniker Name="DotNetAssembly" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="16f750c4-9f39-4fab-a952-5f4a76570339" Description="Description for DSLFactory.Candle.SystemModel.ComponentHasLayerPackages" Name="ComponentHasLayerPackages" DisplayName="Component Has Layer Packages" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="57469885-ee48-498b-ae75-868390cdd0a8" Description="Description for DSLFactory.Candle.SystemModel.ComponentHasLayerPackages.Component" Name="Component" DisplayName="Component" PropertyName="LayerPackages" PropertyDisplayName="Layer Packages">
          <RolePlayer>
            <DomainClassMoniker Name="SoftwareComponent" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="4fe7a91f-2a33-4cda-8464-396a9aada5a5" Description="Description for DSLFactory.Candle.SystemModel.ComponentHasLayerPackages.LayerPackage" Name="LayerPackage" DisplayName="Layer Package" PropertyName="Component" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Component">
          <RolePlayer>
            <DomainClassMoniker Name="LayerPackage" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="fcf8160a-e530-427a-8c67-9ae6e5a55967" Description="Description for DSLFactory.Candle.SystemModel.LayerHasClassImplementations" Name="LayerHasClassImplementations" DisplayName="Layer Has Class Implementations" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="a37ca1c2-5498-4a99-b786-0b59d7cf7312" Description="Description for DSLFactory.Candle.SystemModel.LayerHasClassImplementations.Layer" Name="Layer" DisplayName="Layer" PropertyName="Classes" PropertyDisplayName="Classes">
          <RolePlayer>
            <DomainClassMoniker Name="Layer" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="035ca443-b893-4d13-8b33-4ccece0f0637" Description="Description for DSLFactory.Candle.SystemModel.LayerHasClassImplementations.ClassImplementation" Name="ClassImplementation" DisplayName="Class Implementation" PropertyName="Layer" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Layer">
          <RolePlayer>
            <DomainClassMoniker Name="ClassImplementation" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="25c0f0c1-3ea9-436c-b360-6bf54213ede0" Description="Description for DSLFactory.Candle.SystemModel.Implementation" Name="Implementation" DisplayName="Implementation" Namespace="DSLFactory.Candle.SystemModel">
      <Properties>
        <DomainProperty Id="a0a5dc67-a728-4d88-8688-a46f5cbc66cc" Description="Description for DSLFactory.Candle.SystemModel.Implementation.Name" Name="Name" DisplayName="Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e997bdf3-9a96-473b-aa22-97fae139ac59" Description="Description for DSLFactory.Candle.SystemModel.Implementation.Configuration Mode" Name="ConfigurationMode" DisplayName="Configuration Mode" DefaultValue="*">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="92372c8b-2ba5-41e3-a038-cdca7477ff12" Description="Description for DSLFactory.Candle.SystemModel.Implementation.ClassImplementation" Name="ClassImplementation" DisplayName="Class Implementation" PropertyName="Contract" Multiplicity="ZeroOne" PropertyDisplayName="Contract">
          <RolePlayer>
            <DomainClassMoniker Name="ClassImplementation" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="ca3fb20a-18de-4e0a-8874-3bbd32c9cc29" Description="Description for DSLFactory.Candle.SystemModel.Implementation.Contract" Name="Contract" DisplayName="Contract" PropertyName="Implementations" PropertyDisplayName="Implementations">
          <RolePlayer>
            <DomainClassMoniker Name="ServiceContract" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="eb700931-75b3-41d4-9558-e2e2ce877866" Description="Description for DSLFactory.Candle.SystemModel.ClassImplementationReferencesAssociatedEntity" Name="ClassImplementationReferencesAssociatedEntity" DisplayName="Class Implementation References Associated Entity" Namespace="DSLFactory.Candle.SystemModel">
      <Source>
        <DomainRole Id="e9204b8d-f498-4676-a885-166a4d2384c3" Description="Description for DSLFactory.Candle.SystemModel.ClassImplementationReferencesAssociatedEntity.ClassImplementation" Name="ClassImplementation" DisplayName="Class Implementation" PropertyName="AssociatedEntity" Multiplicity="ZeroOne" PropertyDisplayName="Associated Entity">
          <RolePlayer>
            <DomainClassMoniker Name="ClassImplementation" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="a98d67b7-0d1e-431a-b895-676039be5cd8" Description="Description for DSLFactory.Candle.SystemModel.ClassImplementationReferencesAssociatedEntity.Entity" Name="Entity" DisplayName="Entity" PropertyName="ClassImplementations" IsPropertyGenerator="false" PropertyDisplayName="Class Implementations">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="99173e07-79ff-409d-9236-104c92399c7a" Description="Description for DSLFactory.Candle.SystemModel.ImplementationHasDependencyProperties" Name="ImplementationHasDependencyProperties" DisplayName="Implementation Has Dependency Properties" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="79206d40-c43b-4190-bf51-16a11a9a3ce1" Description="Description for DSLFactory.Candle.SystemModel.ImplementationHasDependencyProperties.Implementation" Name="Implementation" DisplayName="Implementation" PropertyName="DependencyProperties" PropertyDisplayName="Dependency Properties">
          <RolePlayer>
            <DomainRelationshipMoniker Name="Implementation" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="95f909b5-07c6-4219-aa6f-fbd269795bc7" Description="Description for DSLFactory.Candle.SystemModel.ImplementationHasDependencyProperties.DependencyProperty" Name="DependencyProperty" DisplayName="Dependency Property" PropertyName="Implementation" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" PropagatesCopy="true" PropertyDisplayName="Implementation">
          <RolePlayer>
            <DomainClassMoniker Name="DependencyProperty" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="548b0054-8d70-40b8-ad5e-d80e93ef0e20" Description="Description for DSLFactory.Candle.SystemModel.LayerPackageReferencesInterfaceLayer" Name="LayerPackageReferencesInterfaceLayer" DisplayName="Layer Package References Interface Layer" Namespace="DSLFactory.Candle.SystemModel">
      <Source>
        <DomainRole Id="efcd8b86-5624-42de-a399-d5fb2723d1dd" Description="Description for DSLFactory.Candle.SystemModel.LayerPackageReferencesInterfaceLayer.LayerPackage" Name="LayerPackage" DisplayName="Layer Package" PropertyName="InterfaceLayer" Multiplicity="ZeroOne" IsPropertyBrowsable="false" PropertyDisplayName="Interface Layer">
          <RolePlayer>
            <DomainClassMoniker Name="LayerPackage" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="e14683ae-82f0-4d40-8345-9ae5a43138e3" Description="Description for DSLFactory.Candle.SystemModel.LayerPackageReferencesInterfaceLayer.InterfaceLayer" Name="InterfaceLayer" DisplayName="Interface Layer" PropertyName="LayerPackage" Multiplicity="One" IsPropertyBrowsable="false" PropertyDisplayName="Layer Package">
          <RolePlayer>
            <DomainClassMoniker Name="InterfaceLayer" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="e54a56d6-2ea1-45d1-86ac-1558ef556c43" Description="Description for DSLFactory.Candle.SystemModel.LayerHasConfigurationParts" Name="LayerHasConfigurationParts" DisplayName="Layer Has Configuration Parts" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="ddc7735b-c4bb-40c3-932d-85d15cb606d3" Description="Description for DSLFactory.Candle.SystemModel.LayerHasConfigurationParts.AbstractLayer" Name="AbstractLayer" DisplayName="Abstract Layer" PropertyName="Configurations" PropertyDisplayName="Configurations">
          <RolePlayer>
            <DomainClassMoniker Name="AbstractLayer" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="aa44360d-df89-4af7-b74b-0c348bb0224b" Description="Description for DSLFactory.Candle.SystemModel.LayerHasConfigurationParts.ConfigurationPart" Name="ConfigurationPart" DisplayName="Configuration Part" PropertyName="Layer" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Layer">
          <RolePlayer>
            <DomainClassMoniker Name="ConfigurationPart" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="05972ef5-23d1-4f28-8a09-f064e13b7f83" Description="Description for DSLFactory.Candle.SystemModel.ExternalComponentHasPublicPorts" Name="ExternalComponentHasPublicPorts" DisplayName="External Component Has Public Ports" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="cc54120c-4834-47ca-abcf-24c1f7d3b517" Description="Description for DSLFactory.Candle.SystemModel.ExternalComponentHasPublicPorts.Parent" Name="Parent" DisplayName="Parent" PropertyName="Ports" PropertyDisplayName="Ports">
          <RolePlayer>
            <DomainClassMoniker Name="ExternalComponent" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="4a01426b-6c8a-4847-9014-c98830c1704f" Description="Description for DSLFactory.Candle.SystemModel.ExternalComponentHasPublicPorts.Port" Name="Port" DisplayName="Port" PropertyName="Parent" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Parent">
          <RolePlayer>
            <DomainClassMoniker Name="ExternalPublicPort" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="922d6818-d5e2-4d98-96bb-97557dda7060" Description="Description for DSLFactory.Candle.SystemModel.ScenarioUsesContracts" Name="ScenarioUsesContracts" DisplayName="Scenario Uses Contracts" Namespace="DSLFactory.Candle.SystemModel">
      <Properties>
        <DomainProperty Id="99964b02-212d-44f1-97b2-01cd6ca9bb55" Description="Description for DSLFactory.Candle.SystemModel.ScenarioUsesContracts.Name" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="25afcc7a-0385-4b0a-9c77-9de48670e1e5" Description="Description for DSLFactory.Candle.SystemModel.ScenarioUsesContracts.Configuration Mode" Name="ConfigurationMode" DisplayName="Configuration Mode" DefaultValue="*">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="0c9cd39b-bf57-47b7-8b6e-7949f6ce123f" Description="Description for DSLFactory.Candle.SystemModel.ScenarioUsesContracts.Scenario" Name="Scenario" DisplayName="Scenario" PropertyName="Contracts" PropertyDisplayName="Contracts">
          <RolePlayer>
            <DomainClassMoniker Name="Scenario" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="1c92e2df-ac1b-4749-b215-0b4cdc461518" Description="Description for DSLFactory.Candle.SystemModel.ScenarioUsesContracts.Service" Name="Service" DisplayName="Service" PropertyName="Scenarios" IsPropertyGenerator="false" PropertyDisplayName="Scenarios">
          <RolePlayer>
            <DomainClassMoniker Name="ServiceContract" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="5739074f-4878-4c4f-89f4-6b1bd9e6b33f" Description="Description for DSLFactory.Candle.SystemModel.ClassUsesOperations" Name="ClassUsesOperations" DisplayName="Class Uses Operations" Namespace="DSLFactory.Candle.SystemModel">
      <Properties>
        <DomainProperty Id="675d9c8d-e08a-4dd3-a6c7-d8db6e29b673" Description="Description for DSLFactory.Candle.SystemModel.ClassUsesOperations.Name" Name="Name" DisplayName="Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="879f8d42-aaae-482c-ae52-c9486d40d987" Description="Description for DSLFactory.Candle.SystemModel.ClassUsesOperations.Configuration Mode" Name="ConfigurationMode" DisplayName="Configuration Mode" DefaultValue="*">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ca6e1cf4-96f1-404a-869d-306d9f19fc1e" Description="Description for DSLFactory.Candle.SystemModel.ClassUsesOperations.Singleton" Name="Singleton" DisplayName="Singleton">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7b18697c-9d1a-4528-877b-46b4e500f9c2" Description="Description for DSLFactory.Candle.SystemModel.ClassUsesOperations.Scope" Name="Scope" DisplayName="Scope" DefaultValue="Runtime,Publish">
          <Type>
            <ExternalTypeMoniker Name="ReferenceScope" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="8d786d1d-0f86-44d0-bfd3-f7e2ad358363" Description="Description for DSLFactory.Candle.SystemModel.ClassUsesOperations.Source" Name="Source" DisplayName="Source" PropertyName="ServicesUsed" PropertyDisplayName="Services Used">
          <RolePlayer>
            <DomainClassMoniker Name="TypeWithOperations" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="5eb92749-3dc2-4e05-a8b7-2bb331d9ddea" Description="Description for DSLFactory.Candle.SystemModel.ClassUsesOperations.TargetService" Name="TargetService" DisplayName="Target Service" PropertyName="Sources" IsPropertyGenerator="false" PropertyDisplayName="Sources">
          <RolePlayer>
            <DomainClassMoniker Name="NamedElement" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="1068c261-c2b6-4fc5-818c-dfc2c02bb37a" Description="Description for DSLFactory.Candle.SystemModel.ClassUsesOperationsHasDependencyProperties" Name="ClassUsesOperationsHasDependencyProperties" DisplayName="Class Uses Operations Has Dependency Properties" Namespace="DSLFactory.Candle.SystemModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="939a039a-5b54-4199-8d61-9dc5d191092c" Description="Description for DSLFactory.Candle.SystemModel.ClassUsesOperationsHasDependencyProperties.ClassUsesOperations" Name="ClassUsesOperations" DisplayName="Class Uses Operations" PropertyName="DependencyProperties" PropertyDisplayName="Dependency Properties">
          <RolePlayer>
            <DomainRelationshipMoniker Name="ClassUsesOperations" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="774e9344-05c9-4cac-90bf-a34696d5e313" Description="Description for DSLFactory.Candle.SystemModel.ClassUsesOperationsHasDependencyProperties.DependencyProperty" Name="DependencyProperty" DisplayName="Dependency Property" PropertyName="ClassUsesOperations" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" PropagatesCopy="true" PropertyDisplayName="Class Uses Operations">
          <RolePlayer>
            <DomainClassMoniker Name="DependencyProperty" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="016ef407-6928-4cc0-9311-eb1a66cdcfb6" Description="Description for DSLFactory.Candle.SystemModel.DataLayerReferencesExternalComponent" Name="DataLayerReferencesExternalComponent" DisplayName="Models Layer References External Component" Namespace="DSLFactory.Candle.SystemModel">
      <Properties>
        <DomainProperty Id="3ac27700-fbb9-4b8e-b9c1-93039772f4e6" Description="Description for DSLFactory.Candle.SystemModel.DataLayerReferencesExternalComponent.Configuration Mode" Name="ConfigurationMode" DisplayName="Configuration Mode" DefaultValue="*">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b874771e-6203-476d-9825-5dfba70e3ede" Description="Description for DSLFactory.Candle.SystemModel.DataLayerReferencesExternalComponent.Scope" Name="Scope" DisplayName="Scope" DefaultValue="Compilation,Runtime">
          <Type>
            <ExternalTypeMoniker Name="ReferenceScope" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="096abeda-cc95-47ad-8d22-f65c7ef4dd4f" Description="Description for DSLFactory.Candle.SystemModel.DataLayerReferencesExternalComponent.DataLayer" Name="DataLayer" DisplayName="Data Layer" PropertyName="ReferencedExternalComponents" PropertyDisplayName="Referenced External Components">
          <RolePlayer>
            <DomainClassMoniker Name="DataLayer" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="e40af07d-b807-4f99-8fb7-eacd864f3756" Description="Description for DSLFactory.Candle.SystemModel.DataLayerReferencesExternalComponent.ReferencedExternalComponent" Name="ReferencedExternalComponent" DisplayName="Referenced External Component" PropertyName="DataLayer" Multiplicity="ZeroOne" IsPropertyGenerator="false" PropertyDisplayName="Data Layer">
          <RolePlayer>
            <DomainClassMoniker Name="ExternalComponent" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="89bcf941-c818-4315-b2c9-def4d67d1244" Description="Description for DSLFactory.Candle.SystemModel.EntityHasSubClasses" Name="EntityHasSubClasses" DisplayName="Entity Has Sub Classes" Namespace="DSLFactory.Candle.SystemModel" AllowsDuplicates="true">
      <Source>
        <DomainRole Id="d2c31ea6-3629-438b-933e-01efbb4bfb36" Description="Description for DSLFactory.Candle.SystemModel.EntityHasSubClasses.SuperClass" Name="SuperClass" DisplayName="Super Class" PropertyName="EntityHasSubClasses" IsPropertyGenerator="false" PropertyDisplayName="Entity Has Sub Classes">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="a6dd50b0-a7af-4288-b589-686dd2cf123b" Description="Description for DSLFactory.Candle.SystemModel.EntityHasSubClasses.SubClass" Name="SubClass" DisplayName="Sub Class" PropertyName="SuperClasses" PropertyGetterAccessModifier="Assembly" PropertySetterAccessModifier="Assembly" IsPropertyBrowsable="false" PropertyDisplayName="Super Classes">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="5671a244-3eae-4159-ac0d-7a0fa9b6b455" Description="Description for DSLFactory.Candle.SystemModel.Generalization" Name="Generalization" DisplayName="Generalization" Namespace="DSLFactory.Candle.SystemModel">
      <Source>
        <DomainRole Id="0d47735a-4601-4ac9-9b4d-d1d7f952d15a" Description="Description for DSLFactory.Candle.SystemModel.Generalization.SuperClass" Name="SuperClass" DisplayName="Super Class" PropertyName="SubClasses" PropertyDisplayName="Sub Classes">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="fc7095d7-6ecb-449a-8518-0a898f08c0a6" Description="Description for DSLFactory.Candle.SystemModel.Generalization.SubClass" Name="SubClass" DisplayName="Sub Class" PropertyName="SuperClass" Multiplicity="ZeroOne" PropertyDisplayName="Super Class">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
  </Relationships>
  <Types>
    <ExternalType Name="DateTime" Namespace="System" />
    <ExternalType Name="String" Namespace="System" />
    <ExternalType Name="Int16" Namespace="System" />
    <ExternalType Name="Int32" Namespace="System" />
    <ExternalType Name="Int64" Namespace="System" />
    <ExternalType Name="UInt16" Namespace="System" />
    <ExternalType Name="UInt32" Namespace="System" />
    <ExternalType Name="UInt64" Namespace="System" />
    <ExternalType Name="SByte" Namespace="System" />
    <ExternalType Name="Byte" Namespace="System" />
    <ExternalType Name="Double" Namespace="System" />
    <ExternalType Name="Single" Namespace="System" />
    <ExternalType Name="Guid" Namespace="System" />
    <ExternalType Name="Boolean" Namespace="System" />
    <ExternalType Name="Char" Namespace="System" />
    <ExternalType Name="Version" Namespace="System" />
    <ExternalType Name="List&lt;string&gt;" Namespace="System.Collections.Generic" />
    <ExternalType Name="List&lt;Guid&gt;" Namespace="System.Collections.Generic" />
    <DomainEnumeration Name="Multiplicity" Namespace="DSLFactory.Candle.SystemModel" Description="Description for DSLFactory.Candle.SystemModel.Multiplicity">
      <Literals>
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.Multiplicity.One" Name="One" Value="" />
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.Multiplicity.OneMany" Name="OneMany" Value="" />
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.Multiplicity.ZeroMany" Name="ZeroMany" Value="" />
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.Multiplicity.ZeroOne" Name="ZeroOne" Value="" />
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.Multiplicity.NotApplicable" Name="NotApplicable" Value="" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="VersionInfo" Namespace="DSLFactory.Candle.SystemModel" />
    <DomainEnumeration Name="ArtifactType" Namespace="DSLFactory.Candle.SystemModel" Description="Artifact type">
      <Literals>
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.ArtifactType.Assembly" Name="Assembly" Value="" />
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.ArtifactType.Content" Name="Content" Value="2" />
        <EnumerationLiteral Description="Assembly dans le GAC" Name="AssemblyInGac" Value="1" />
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.ArtifactType.DotNetFramework" Name="DotNetFramework" Value="4" />
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.ArtifactType.Project" Name="Project" Value="3" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="SpecificNameProvider" Namespace="DSLFactory.Candle.SystemModel.NameProvider" />
    <DomainEnumeration Name="AssociationSort" Namespace="DSLFactory.Candle.SystemModel" Description="Description for DSLFactory.Candle.SystemModel.AssociationSort">
      <Literals>
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.AssociationSort.Composition" Name="Composition" Value="3" />
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.AssociationSort.Aggregation" Name="Aggregation" Value="1" />
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.AssociationSort.Normal" Name="Normal" Value="2" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="ReferenceScope" Namespace="DSLFactory.Candle.SystemModel" />
    <DomainEnumeration Name="Visibility" Namespace="DSLFactory.Candle.SystemModel" Description="Visibility">
      <Literals>
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.Visibility.Private" Name="Private" Value="" />
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.Visibility.Public" Name="Public" Value="" />
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.Visibility.Internal" Name="Internal" Value="" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="Color" Namespace="System.Drawing" />
    <DomainEnumeration Name="ArgumentDirection" Namespace="DSLFactory.Candle.SystemModel" Description="Argument direction">
      <Literals>
        <EnumerationLiteral Description="" Name="InOut" Value="" />
        <EnumerationLiteral Description="" Name="In" Value="" />
        <EnumerationLiteral Description="" Name="Out" Value="" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="DatabaseType" Namespace="DSLFactory.Candle.SystemModel" Description="Description for DSLFactory.Candle.SystemModel.DatabaseType">
      <Literals>
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.DatabaseType.Table" Name="Table" Value="" />
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.DatabaseType.StoredProcedure" Name="StoredProcedure" Value="" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="HostingContext" Namespace="DSLFactory.Candle.SystemModel" Description="Description for DSLFactory.Candle.SystemModel.HostingContext">
      <Literals>
        <EnumerationLiteral Description="Cette couche n'est pas hostÃ©e" Name="None" Value="" />
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.HostingContext.Standalone" Name="Standalone" Value="" />
        <EnumerationLiteral Description="Description for DSLFactory.Candle.SystemModel.HostingContext.Web" Name="Web" Value="" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="DependencyPropertyValue" Namespace="DSLFactory.Candle.SystemModel.Strategies" />
    <ExternalType Name="ComponentType" Namespace="DSLFactory.Candle.SystemModel" />
  </Types>
  <Shapes>
    <GeometryShape Id="44aff69a-3830-41ac-8164-f87d6eb725ba" Description="Description for DSLFactory.Candle.SystemModel.SoftwareComponentShape" Name="SoftwareComponentShape" DisplayName="Software Component Shape" Namespace="DSLFactory.Candle.SystemModel" GeneratesDoubleDerived="true" FixedTooltipText="Software Component Shape" TextColor="DarkGray" OutlineColor="231, 200, 136" InitialWidth="7" InitialHeight="6" OutlineDashStyle="Dash" OutlineThickness="0.03" FillGradientMode="None" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="HeaderText" DisplayName="Header Text" DefaultText="HeaderText" FontStyle="Bold, Italic" FontSize="13" />
      </ShapeHasDecorators>
    </GeometryShape>
    <GeometryShape Id="9eb28bfd-1f70-4d1f-8aac-193dfc870d7a" Description="Description for DSLFactory.Candle.SystemModel.BusinessLayerShape" Name="BusinessLayerShape" DisplayName="Business Layer Shape" Namespace="DSLFactory.Candle.SystemModel" GeneratesDoubleDerived="true" FixedTooltipText="Business Layer Shape" OutlineColor="148, 182, 247" InitialWidth="6" InitialHeight="1" OutlineThickness="0.02" FillGradientMode="None" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" FontStyle="Bold" FontSize="10" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerMiddleRight" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="IconDecorator1" DisplayName="BLL" DefaultIcon="Resources\Tools.png" />
      </ShapeHasDecorators>
    </GeometryShape>
    <GeometryShape Id="38842893-12b3-436d-85da-d829c179422f" Description="Description for DSLFactory.Candle.SystemModel.DataAccessLayerShape" Name="DataAccessLayerShape" DisplayName="Data Access Layer Shape" Namespace="DSLFactory.Candle.SystemModel" GeneratesDoubleDerived="true" FixedTooltipText="Data Access Layer Shape" OutlineColor="148, 182, 247" InitialWidth="6" InitialHeight="1" OutlineThickness="0.02" FillGradientMode="None" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="NameDecorator" DefaultText="NameDecorator" FontStyle="Bold" FontSize="10" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerMiddleRight" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="IconDecorator" DisplayName="Database icon" DefaultIcon="Resources\DalLayerImage.gif" />
      </ShapeHasDecorators>
    </GeometryShape>
    <GeometryShape Id="02087811-61e6-48de-a619-9ffbe5ec89c8" Description="Description for DSLFactory.Candle.SystemModel.PresentationLayerShape" Name="PresentationLayerShape" DisplayName="Presentation Layer Shape" Namespace="DSLFactory.Candle.SystemModel" GeneratesDoubleDerived="true" FixedTooltipText="Presentation Layer Shape" OutlineColor="148, 182, 247" InitialWidth="6" InitialHeight="1" OutlineThickness="0.02" FillGradientMode="None" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="NameDecorator" DefaultText="NameDecorator" FontStyle="Bold" FontSize="10" />
      </ShapeHasDecorators>
    </GeometryShape>
    <GeometryShape Id="173fd410-f787-4b80-9f8b-60d09a1641d3" Description="Description for DSLFactory.Candle.SystemModel.DataLayerShape" Name="DataLayerShape" DisplayName="Models Layer Shape" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="Models Layer Shape" FillColor="166, 196, 247" OutlineColor="231, 200, 136" InitialWidth="1" InitialHeight="4" OutlineDashStyle="Dash" OutlineThickness="0.025" FillGradientMode="Vertical" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="NameDecorator" DefaultText="NameDecorator" FontStyle="Bold" FontSize="10" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerBottomCenter" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="IconDecorator1" DisplayName="Models" DefaultIcon="Resources\doc.png" />
      </ShapeHasDecorators>
    </GeometryShape>
    <GeometryShape Id="fd27ac3d-9fb2-4a77-ab40-52f547872a8c" Description="Description for DSLFactory.Candle.SystemModel.PackageShape" Name="PackageShape" DisplayName="Package Shape" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="Package Shape" InitialWidth="3" InitialHeight="3" OutlineDashStyle="Dot" OutlineThickness="0.02125" FillGradientMode="None" Geometry="Rectangle">
      <ShapeHasDecorators Position="OuterTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name" DefaultText="NameDecorator" />
      </ShapeHasDecorators>
    </GeometryShape>
    <GeometryShape Id="0e92e8a5-2da7-4fed-8606-2a8864912bdf" Description="Description for DSLFactory.Candle.SystemModel.ExternalComponentShape" Name="ExternalComponentShape" DisplayName="External Component Shape" Namespace="DSLFactory.Candle.SystemModel" GeneratesDoubleDerived="true" FixedTooltipText="External Component Shape" FillColor="102, 129, 166" OutlineColor="RoyalBlue" InitialWidth="1" InitialHeight="1.5" OutlineThickness="0.04" FillGradientMode="Vertical" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="InnerBottomCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="VersionDecorator" DisplayName="Version" DefaultText="-1" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="IsLastVersionDecorator" DisplayName="IsLastVersionDecorator" DefaultIcon="Resources\Error.bmp" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="Center" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" />
      </ShapeHasDecorators>
    </GeometryShape>
    <Port Id="169a580a-714a-451b-8f7f-040064acb5da" Description="Description for DSLFactory.Candle.SystemModel.ExternalPublicPortShape" Name="ExternalPublicPortShape" DisplayName="External Public Port Shape" Namespace="DSLFactory.Candle.SystemModel" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="External Public Port Shape" InitialWidth="0.98" InitialHeight="0.15" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerMiddleLeft" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Interface name" DefaultText="interface" />
      </ShapeHasDecorators>
    </Port>
    <CompartmentShape Id="d19ac0e3-392b-40e6-acc9-9faff31b00fc" Description="Description for DSLFactory.Candle.SystemModel.EntityShape" Name="EntityShape" DisplayName="Entity Shape" Namespace="DSLFactory.Candle.SystemModel" GeneratesDoubleDerived="true" TooltipType="Fixed" FixedTooltipText="doucle click to go to source or Ctrl+double click to resize" FillColor="255, 128, 0" InitialHeight="0.3" OutlineThickness="0.01" FillGradientMode="Vertical" Geometry="RoundedRectangle">
      <CustomTypeDescriptor>
        <DomainTypeDescriptor />
      </CustomTypeDescriptor>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <ExpandCollapseDecorator Name="ExpandCollapseDecorator1" DisplayName="Expand Collapse Decorator1" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.01" VerticalOffset="0.01">
        <IconDecorator Name="IconDecorator1" DisplayName="Icon Decorator1" DefaultIcon="Resources\VSObject_Class.bmp" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.21" VerticalOffset="0.01">
        <TextDecorator Name="NameDecorator" DisplayName="NameDecorator" DefaultText="NameDecorator" FontStyle="Bold" />
      </ShapeHasDecorators>
      <Compartment Name="Properties" Title="Properties" />
    </CompartmentShape>
    <CompartmentShape Id="42c5e0c1-c513-43fe-934d-f4936360aabe" Description="Description for DSLFactory.Candle.SystemModel.EnumTypeShape" Name="EnumTypeShape" DisplayName="Enum Type Shape" Namespace="DSLFactory.Candle.SystemModel" TooltipType="Fixed" FixedTooltipText="doucle click to got to source or Ctrl+double click to resize" FillColor="DodgerBlue" InitialHeight="0.3" OutlineThickness="0.01" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <ExpandCollapseDecorator Name="ExpandCollapseDecorator1" DisplayName="Expand Collapse Decorator1" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.01" VerticalOffset="0.01">
        <IconDecorator Name="IconDecorator1" DisplayName="Icon Decorator1" DefaultIcon="Resources\VSObject_Enum.bmp" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.21" VerticalOffset="0.01">
        <TextDecorator Name="NameDecorator" DisplayName="NameDecorator" DefaultText="NameDecorator" FontStyle="Bold" />
      </ShapeHasDecorators>
      <Compartment Name="Values" Title="Values" />
    </CompartmentShape>
    <GeometryShape Id="80c8f77b-8717-42ea-b75f-6a06f80261ae" Description="Description for DSLFactory.Candle.SystemModel.ScenarioShape" Name="ScenarioShape" DisplayName="Scenario Shape" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="Scenario Shape" OutlineColor="128, 128, 255" InitialWidth="3" InitialHeight="3" OutlineDashStyle="Dash" OutlineThickness="0.03" FillGradientMode="None" Geometry="Rectangle">
      <ShapeHasDecorators Position="OuterTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" />
      </ShapeHasDecorators>
    </GeometryShape>
    <GeometryShape Id="60071cd6-9bda-4eda-8d7c-deb0a520a5d1" Description="Description for DSLFactory.Candle.SystemModel.UiViewShape" Name="UiViewShape" DisplayName="Ui View Shape" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="Ui View Shape" FillColor="192, 192, 255" InitialHeight="1" OutlineThickness="0.02" FillGradientMode="Vertical" Geometry="Rectangle">
      <ShapeHasDecorators Position="Center" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" />
      </ShapeHasDecorators>
    </GeometryShape>
    <GeometryShape Id="93d27478-d382-4f03-a0a5-cd648fe37232" Description="Description for DSLFactory.Candle.SystemModel.UIWorkflowLayerShape" Name="UIWorkflowLayerShape" DisplayName="UIWorkflow Layer Shape" Namespace="DSLFactory.Candle.SystemModel" GeneratesDoubleDerived="true" FixedTooltipText="UIWorkflow Layer Shape" OutlineColor="148, 182, 247" InitialWidth="6" InitialHeight="1" OutlineThickness="0.02" FillGradientMode="None" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" FontStyle="Bold" FontSize="10" />
      </ShapeHasDecorators>
    </GeometryShape>
    <GeometryShape Id="785bee42-52c3-4ef3-9af1-cb6c2675b073" Description="Description for DSLFactory.Candle.SystemModel.DotnetAssemblyShape" Name="DotnetAssemblyShape" DisplayName="Dotnet Assembly Shape" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="Dotnet Assembly Shape" FillColor="250, 230, 177" OutlineColor="Gainsboro" InitialWidth="2" InitialHeight="0.5" OutlineDashStyle="Dash" OutlineThickness="0.03" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="NameDecorator" DefaultText="" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="IconDecorator1" DisplayName="Icon Decorator1" DefaultIcon="Resources\RootClassIcon.bmp" />
      </ShapeHasDecorators>
    </GeometryShape>
    <GeometryShape Id="2a3a91d3-695a-4a09-9398-f0a534c2693e" Description="Description for DSLFactory.Candle.SystemModel.BinaryComponentShape" Name="BinaryComponentShape" DisplayName="Binary Component Shape" Namespace="DSLFactory.Candle.SystemModel" GeneratesDoubleDerived="true" FixedTooltipText="Binary Component Shape" FillColor="WhiteSmoke" InitialWidth="5" InitialHeight="4" OutlineDashStyle="Dot" OutlineThickness="0.025" FillGradientMode="None" Geometry="Rectangle" />
    <GeometryShape Id="d723b63c-3c50-4074-bc0f-e40069f63531" Description="Description for DSLFactory.Candle.SystemModel.LayerPackageShape" Name="LayerPackageShape" DisplayName="Layer Package Shape" Namespace="DSLFactory.Candle.SystemModel" GeneratesDoubleDerived="true" FixedTooltipText="Layer Package Shape" FillColor="166, 196, 247" OutlineColor="SteelBlue" InitialWidth="7" InitialHeight="1.5" OutlineDashStyle="Dash" OutlineThickness="0.025" FillGradientMode="None" Geometry="Rectangle" />
    <GeometryShape Id="2face4f4-6230-4bd8-94a2-869ba5d0b6fe" Description="Description for DSLFactory.Candle.SystemModel.InterfaceLayerShape" Name="InterfaceLayerShape" DisplayName="Interface Layer Shape" InheritanceModifier="Sealed" Namespace="DSLFactory.Candle.SystemModel" GeneratesDoubleDerived="true" FixedTooltipText="Interface Layer Shape" FillColor="Silver" OutlineColor="177, 189, 224" InitialWidth="7" InitialHeight="1" OutlineThickness="0.02" FillGradientMode="Vertical" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" FontStyle="Bold" FontSize="10" />
      </ShapeHasDecorators>
    </GeometryShape>
    <CompartmentShape Id="1c26da56-8ff1-40fd-bbe7-e52e94fd9b80" Description="Description for DSLFactory.Candle.SystemModel.ServiceContractShape" Name="ServiceContractShape" DisplayName="Service Contract Shape" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="Service Contract Shape" FillColor="Gray" OutlineColor="DimGray" InitialHeight="0.3" OutlineDashStyle="Dash" OutlineThickness="0.01" Geometry="RoundedRectangle" DefaultExpandCollapseState="Collapsed">
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="TextDecorator1" DisplayName="Text Decorator1" DefaultText="TextDecorator1" FontStyle="Bold, Italic" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="IconDecorator1" DisplayName="Icon Decorator1" DefaultIcon="Resources\VSObject_Interface.bmp" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <ExpandCollapseDecorator Name="ExpandCollapseDecorator1" DisplayName="Expand Collapse Decorator1" />
      </ShapeHasDecorators>
      <Compartment Name="Operations" DefaultExpandCollapseState="Collapsed" Title="Operations" />
    </CompartmentShape>
    <CompartmentShape Id="57efc17b-2c10-4fe9-8d50-90d72c3b3588" Description="Description for DSLFactory.Candle.SystemModel.ClassImplementationShape" Name="ClassImplementationShape" DisplayName="Class Implementation Shape" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="Class Implementation Shape" FillColor="SteelBlue" InitialHeight="0.3" OutlineThickness="0.01" Geometry="RoundedRectangle" DefaultExpandCollapseState="Collapsed">
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="TextDecorator1" DisplayName="Text Decorator1" DefaultText="TextDecorator1" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="IconDecorator1" DisplayName="Icon Decorator1" DefaultIcon="Resources\VSObject_Class.bmp" />
      </ShapeHasDecorators>
    </CompartmentShape>
    <GeometryShape Id="528a3026-1029-4795-bcf3-084786e4c2b1" Description="Description for DSLFactory.Candle.SystemModel.ProcessShape" Name="ProcessShape" DisplayName="Process Shape" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="Process Shape" FillColor="MediumAquamarine" InitialWidth="0.7" InitialHeight="0.7" Geometry="Rectangle" />
    <Port Id="4ced83da-8026-4112-b4fb-26fabde8bd34" Description="Description for DSLFactory.Candle.SystemModel.ExternalServiceContractShape" Name="ExternalServiceContractShape" DisplayName="External Service Contract Shape" Namespace="DSLFactory.Candle.SystemModel" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="External Service Contract Shape" InitialWidth="0.98" InitialHeight="0.15" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerMiddleLeft" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" />
      </ShapeHasDecorators>
    </Port>
    <GeometryShape Id="b562ce84-6e8e-4ae1-89c4-4c4c57dd6af8" Description="Description for DSLFactory.Candle.SystemModel.ScenarioThumbnailShape" Name="ScenarioThumbnailShape" DisplayName="Scenario Thumbnail Shape" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="Scenario Thumbnail Shape" InitialWidth="0.5" InitialHeight="0.5" Geometry="Rectangle">
      <BaseGeometryShape>
        <GeometryShapeMoniker Name="ScenarioShape" />
      </BaseGeometryShape>
    </GeometryShape>
  </Shapes>
  <Connectors>
    <Connector Id="309a88dc-aba3-4966-b327-6805810f2190" Description="Description for DSLFactory.Candle.SystemModel.AssociationLink" Name="AssociationLink" DisplayName="Association Link" Namespace="DSLFactory.Candle.SystemModel" GeneratesDoubleDerived="true" TooltipType="Fixed" FixedTooltipText="Alt+clic to show associated fields or double click for details" DashStyle="Dash" TargetEndStyle="EmptyArrow" Thickness="0.02">
      <ConnectorHasDecorators Position="SourceTop" OffsetFromShape="0" OffsetFromLine="0">
        <TextDecorator Name="SourceDecorator" DisplayName="Source Decorator" DefaultText="SourceDecorator" />
      </ConnectorHasDecorators>
      <ConnectorHasDecorators Position="TargetTop" OffsetFromShape="0" OffsetFromLine="0">
        <TextDecorator Name="TargetDecorator" DisplayName="Target Decorator" DefaultText="TargetDecorator" />
      </ConnectorHasDecorators>
      <ConnectorHasDecorators Position="SourceBottom" OffsetFromShape="0" OffsetFromLine="0">
        <TextDecorator Name="SourceMultiplicityDecorator" DisplayName="Source Multiplicity Decorator" DefaultText="SourceMultiplicityDecorator" />
      </ConnectorHasDecorators>
      <ConnectorHasDecorators Position="TargetBottom" OffsetFromShape="0" OffsetFromLine="0">
        <TextDecorator Name="TargetMultiplicityDecorator" DisplayName="Target Multiplicity Decorator" DefaultText="TargetMultiplicityDecorator" />
      </ConnectorHasDecorators>
    </Connector>
    <Connector Id="2f920d65-4272-4fc2-b6a9-dfea1789d2a6" Description="Description for DSLFactory.Candle.SystemModel.ActionLink" Name="ActionLink" DisplayName="Action Link" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="Action Link" TargetEndStyle="FilledArrow" Thickness="0.02">
      <ConnectorHasDecorators Position="SourceTop" OffsetFromShape="0" OffsetFromLine="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="&lt;&lt;not defined&gt;&gt;" />
      </ConnectorHasDecorators>
    </Connector>
    <Connector Id="5fcb64e0-f8b4-426e-8aee-315c43920c2e" Description="Description for DSLFactory.Candle.SystemModel.AssemblyReferencesAssemblyLink" Name="AssemblyReferencesAssemblyLink" DisplayName="Assembly References Assembly Link" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="Assembly References Assembly Link" DashStyle="Dash" TargetEndStyle="EmptyArrow" Thickness="0.015" />
    <Connector Id="2313e5ea-c3a4-4b29-8ddc-cbfe2d40def0" Description="Description for DSLFactory.Candle.SystemModel.ImplementationLink" Name="ImplementationLink" DisplayName="Implementation Link" Namespace="DSLFactory.Candle.SystemModel" GeneratesDoubleDerived="true" FixedTooltipText="Implementation Link" TextColor="White" Color="RoyalBlue" DashStyle="Dash" TargetEndStyle="HollowArrow" Thickness="0.02" RoutingStyle="Straight" />
    <Connector Id="330f665a-e59a-4d3e-9932-bbccda79b352" Description="Description for DSLFactory.Candle.SystemModel.ExternalServiceReferenceLink" Name="ExternalServiceReferenceLink" DisplayName="External Service Reference Link" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="External Service Reference Link" TextColor="Gray" Color="Gray" DashStyle="Dash" TargetEndStyle="EmptyArrow" Thickness="0.025" />
    <Connector Id="00964c5e-5525-4fd9-9893-940b212e2215" Description="Description for DSLFactory.Candle.SystemModel.ClassUsesOperationLink" Name="ClassUsesOperationLink" DisplayName="Class Uses Operation Link" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="Class Uses Operation Link" Color="255, 128, 0" TargetEndStyle="FilledArrow" Thickness="0.02" />
    <Connector Id="44b9dec3-c0d7-4a85-8a04-6c7c7d19edc1" Description="Description for DSLFactory.Candle.SystemModel.ScenarioUsesContractsLink" Name="ScenarioUsesContractsLink" DisplayName="Scenario Uses Contracts Link" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="Scenario Uses Contracts Link" Color="255, 128, 0" TargetEndStyle="FilledArrow" Thickness="0.02" />
    <Connector Id="2313a3c9-98c7-4c74-9b64-f8adba49f6cc" Description="Description for DSLFactory.Candle.SystemModel.DataLayerReferencesExternalComponentLink" Name="DataLayerReferencesExternalComponentLink" DisplayName="Models Layer References External Component Link" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="Models Layer References External Component Link" TextColor="LightSteelBlue" DashStyle="Dash" TargetEndStyle="EmptyArrow" Thickness="0.025" />
    <Connector Id="d36b3712-5299-4cbe-bd88-184310bf1be9" Description="Description for DSLFactory.Candle.SystemModel.GeneralizationLink" Name="GeneralizationLink" DisplayName="Generalization Link" Namespace="DSLFactory.Candle.SystemModel" FixedTooltipText="Generalization Link" DashStyle="Dot" TargetEndStyle="HollowArrow" />
  </Connectors>
  <XmlSerializationBehavior Name="CandleSerializationBehavior" Namespace="DSLFactory.Candle.SystemModel">
    <ClassData>
      <XmlClassData TypeName="CandleModel" MonikerAttributeName="" SerializeId="true" MonikerElementName="modelRootMoniker" ElementName="componentModel" MonikerTypeName="ModelRootMoniker">
        <DomainClassMoniker Name="CandleModel" />
        <ElementData>
          <XmlPropertyData XmlName="path">
            <DomainPropertyMoniker Name="CandleModel/Path" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="url">
            <DomainPropertyMoniker Name="CandleModel/Url" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="version">
            <DomainPropertyMoniker Name="CandleModel/Version" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="externalComponents">
            <DomainRelationshipMoniker Name="CandleModelHasExternalComponents" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="strategyTemplate">
            <DomainPropertyMoniker Name="CandleModel/StrategyTemplate" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dotNetFrameworkVersion">
            <DomainPropertyMoniker Name="CandleModel/DotNetFrameworkVersion" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="visibility">
            <DomainPropertyMoniker Name="CandleModel/Visibility" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="componentType" Representation="Ignore">
            <DomainPropertyMoniker Name="CandleModel/ComponentType" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="component">
            <DomainRelationshipMoniker Name="ModelRootHasComponent" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="isLibrary">
            <DomainPropertyMoniker Name="CandleModel/IsLibrary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="baseAddress">
            <DomainPropertyMoniker Name="CandleModel/BaseAddress" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="SoftwareComponent" MonikerAttributeName="" SerializeId="true" MonikerElementName="softwareComponentMoniker" ElementName="softwareComponent" MonikerTypeName="SoftwareComponentMoniker">
        <DomainClassMoniker Name="SoftwareComponent" />
        <ElementData>
          <XmlRelationshipData RoleElementName="layers">
            <DomainRelationshipMoniker Name="SoftwareComponentHasLayers" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="layerPackages">
            <DomainRelationshipMoniker Name="ComponentHasLayerPackages" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ComponentModelDiagram" MonikerAttributeName="" MonikerElementName="componentModelDiagramMoniker" ElementName="componentModelDiagram" MonikerTypeName="ComponentModelDiagramMoniker">
        <DiagramMoniker Name="ComponentModelDiagram" />
      </XmlClassData>
      <XmlClassData TypeName="NamedElement" MonikerAttributeName="" MonikerElementName="namedElementMoniker" ElementName="namedElement" MonikerTypeName="NamedElementMoniker">
        <DomainClassMoniker Name="NamedElement" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="NamedElement/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="comment" Representation="Element">
            <DomainPropertyMoniker Name="NamedElement/Comment" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="SoftwareComponentShape" MonikerAttributeName="" MonikerElementName="softwareComponentShapeMoniker" ElementName="softwareComponentShape" MonikerTypeName="SoftwareComponentShapeMoniker">
        <GeometryShapeMoniker Name="SoftwareComponentShape" />
      </XmlClassData>
      <XmlClassData TypeName="BusinessLayerShape" MonikerAttributeName="" MonikerElementName="businessLayerShapeMoniker" ElementName="businessLayerShape" MonikerTypeName="BusinessLayerShapeMoniker">
        <GeometryShapeMoniker Name="BusinessLayerShape" />
      </XmlClassData>
      <XmlClassData TypeName="BusinessLayer" MonikerAttributeName="" SerializeId="true" MonikerElementName="businessLayerMoniker" ElementName="businessLayer" MonikerTypeName="BusinessLayerMoniker">
        <DomainClassMoniker Name="BusinessLayer" />
      </XmlClassData>
      <XmlClassData TypeName="DataAccessLayerShape" MonikerAttributeName="" MonikerElementName="dataAccessLayerShapeMoniker" ElementName="dataAccessLayerShape" MonikerTypeName="DataAccessLayerShapeMoniker">
        <GeometryShapeMoniker Name="DataAccessLayerShape" />
      </XmlClassData>
      <XmlClassData TypeName="DataAccessLayer" MonikerAttributeName="" SerializeId="true" MonikerElementName="dataAccessLayerMoniker" ElementName="dataAccessLayer" MonikerTypeName="DataAccessLayerMoniker">
        <DomainClassMoniker Name="DataAccessLayer" />
      </XmlClassData>
      <XmlClassData TypeName="PresentationLayer" MonikerAttributeName="" SerializeId="true" MonikerElementName="presentationLayerMoniker" ElementName="presentationLayer" MonikerTypeName="PresentationLayerMoniker">
        <DomainClassMoniker Name="PresentationLayer" />
      </XmlClassData>
      <XmlClassData TypeName="PresentationLayerShape" MonikerAttributeName="" MonikerElementName="presentationLayerShapeMoniker" ElementName="presentationLayerShape" MonikerTypeName="PresentationLayerShapeMoniker">
        <GeometryShapeMoniker Name="PresentationLayerShape" />
      </XmlClassData>
      <XmlClassData TypeName="CandleElement" MonikerAttributeName="" SerializeId="true" MonikerElementName="candleElementMoniker" ElementName="candleElement" MonikerTypeName="CandleElementMoniker">
        <DomainClassMoniker Name="CandleElement" />
        <ElementData>
          <XmlRelationshipData RoleElementName="dependencyProperties">
            <DomainRelationshipMoniker Name="ElementHasDependencyProperties" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="rootName">
            <DomainPropertyMoniker Name="CandleElement/RootName" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DataLayerShape" MonikerAttributeName="" MonikerElementName="modelsLayerShapeMoniker" ElementName="modelsLayerShape" MonikerTypeName="DataLayerShapeMoniker">
        <GeometryShapeMoniker Name="DataLayerShape" />
      </XmlClassData>
      <XmlClassData TypeName="TypeMember" MonikerAttributeName="" SerializeId="true" MonikerElementName="typeMemberMoniker" ElementName="typeMember" MonikerTypeName="TypeMemberMoniker">
        <DomainClassMoniker Name="TypeMember" />
        <ElementData>
          <XmlPropertyData XmlName="type">
            <DomainPropertyMoniker Name="TypeMember/Type" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isCollection">
            <DomainPropertyMoniker Name="TypeMember/IsCollection" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="displayName" Representation="Ignore">
            <DomainPropertyMoniker Name="TypeMember/DisplayName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="xmlName">
            <DomainPropertyMoniker Name="TypeMember/XmlName" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Argument" MonikerAttributeName="" SerializeId="true" MonikerElementName="argumentMoniker" ElementName="argument" MonikerTypeName="ArgumentMoniker">
        <DomainClassMoniker Name="Argument" />
        <ElementData>
          <XmlPropertyData XmlName="direction">
            <DomainPropertyMoniker Name="Argument/Direction" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Operation" MonikerAttributeName="" SerializeId="true" MonikerElementName="operationMoniker" ElementName="operation" MonikerTypeName="OperationMoniker">
        <DomainClassMoniker Name="Operation" />
        <ElementData>
          <XmlRelationshipData RoleElementName="arguments">
            <DomainRelationshipMoniker Name="OperationHasArguments" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="customAttributes">
            <DomainPropertyMoniker Name="Operation/CustomAttributes" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="OperationHasArguments" MonikerAttributeName="" MonikerElementName="operationHasArgumentsMoniker" ElementName="operationHasArguments" MonikerTypeName="OperationHasArgumentsMoniker">
        <DomainRelationshipMoniker Name="OperationHasArguments" />
      </XmlClassData>
      <XmlClassData TypeName="Package" MonikerAttributeName="" SerializeId="true" MonikerElementName="packageMoniker" ElementName="package" MonikerTypeName="PackageMoniker">
        <DomainClassMoniker Name="Package" />
        <ElementData>
          <XmlRelationshipData RoleElementName="types">
            <DomainRelationshipMoniker Name="PackageHasTypes" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DataType" MonikerAttributeName="" SerializeId="true" MonikerElementName="dataTypeMoniker" ElementName="dataType" MonikerTypeName="DataTypeMoniker">
        <DomainClassMoniker Name="DataType" />
      </XmlClassData>
      <XmlClassData TypeName="PackageHasTypes" MonikerAttributeName="" MonikerElementName="packageHasTypesMoniker" ElementName="packageHasTypes" MonikerTypeName="PackageHasTypesMoniker">
        <DomainRelationshipMoniker Name="PackageHasTypes" />
      </XmlClassData>
      <XmlClassData TypeName="DataLayerHasPackages" MonikerAttributeName="" MonikerElementName="dataLayerHasPackagesMoniker" ElementName="dataLayerHasPackages" MonikerTypeName="DataLayerHasPackagesMoniker">
        <DomainRelationshipMoniker Name="DataLayerHasPackages" />
      </XmlClassData>
      <XmlClassData TypeName="Property" MonikerAttributeName="" SerializeId="true" MonikerElementName="propertyMoniker" ElementName="property" MonikerTypeName="PropertyMoniker">
        <DomainClassMoniker Name="Property" />
        <ElementData>
          <XmlPropertyData XmlName="nullable">
            <DomainPropertyMoniker Name="Property/Nullable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isPrimaryKey">
            <DomainPropertyMoniker Name="Property/IsPrimaryKey" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="customAttributes">
            <DomainPropertyMoniker Name="Property/CustomAttributes" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="columnName">
            <DomainPropertyMoniker Name="Property/ColumnName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="serverType">
            <DomainPropertyMoniker Name="Property/ServerType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isAutoIncrement">
            <DomainPropertyMoniker Name="Property/IsAutoIncrement" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Association" MonikerAttributeName="" SerializeId="true" MonikerElementName="associationMoniker" ElementName="association" MonikerTypeName="AssociationMoniker">
        <DomainRelationshipMoniker Name="Association" />
        <ElementData>
          <XmlPropertyData XmlName="xmlName">
            <DomainPropertyMoniker Name="Association/XmlName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourceRoleName">
            <DomainPropertyMoniker Name="Association/SourceRoleName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetRoleName">
            <DomainPropertyMoniker Name="Association/TargetRoleName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetMultiplicity">
            <DomainPropertyMoniker Name="Association/TargetMultiplicity" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourceMultiplicity">
            <DomainPropertyMoniker Name="Association/SourceMultiplicity" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sort">
            <DomainPropertyMoniker Name="Association/Sort" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="dependencyProperties">
            <DomainRelationshipMoniker Name="AssociationHasProperties" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="foreignKeys">
            <DomainRelationshipMoniker Name="AssociationHasForeignKeys" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="PackageShape" MonikerAttributeName="" MonikerElementName="packageShapeMoniker" ElementName="packageShape" MonikerTypeName="PackageShapeMoniker">
        <GeometryShapeMoniker Name="PackageShape" />
      </XmlClassData>
      <XmlClassData TypeName="ExternalComponent" MonikerAttributeName="" SerializeId="true" MonikerElementName="externalComponentMoniker" ElementName="externalComponent" MonikerTypeName="ExternalComponentMoniker">
        <DomainClassMoniker Name="ExternalComponent" />
        <ElementData>
          <XmlPropertyData XmlName="version">
            <DomainPropertyMoniker Name="ExternalComponent/Version" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="ports">
            <DomainRelationshipMoniker Name="ExternalComponentHasPublicPorts" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="modelMoniker">
            <DomainPropertyMoniker Name="ExternalComponent/ModelMoniker" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isLastVersion" Representation="Ignore">
            <DomainPropertyMoniker Name="ExternalComponent/IsLastVersion" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="ExternalComponent/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="ExternalComponent/Namespace" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="CandleModelHasExternalComponents" MonikerAttributeName="" MonikerElementName="modelRootHasExternalComponentsMoniker" ElementName="modelRootHasExternalComponents" MonikerTypeName="ModelRootHasExternalComponentsMoniker">
        <DomainRelationshipMoniker Name="CandleModelHasExternalComponents" />
      </XmlClassData>
      <XmlClassData TypeName="DependencyProperty" MonikerAttributeName="" MonikerElementName="DependencyPropertyValueMoniker" ElementName="strategyProperty" MonikerTypeName="DependencyPropertyMoniker">
        <DomainClassMoniker Name="DependencyProperty" />
        <ElementData>
          <XmlPropertyData XmlName="strategyId">
            <DomainPropertyMoniker Name="DependencyProperty/StrategyId" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="DependencyProperty/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="value" Representation="Element">
            <DomainPropertyMoniker Name="DependencyProperty/Value" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ExternalPublicPort" MonikerAttributeName="" SerializeId="true" MonikerElementName="externalPublicPortMoniker" ElementName="externalPublicPort" MonikerTypeName="ExternalPublicPortMoniker">
        <DomainClassMoniker Name="ExternalPublicPort" />
        <ElementData>
          <XmlPropertyData XmlName="componentPortMoniker">
            <DomainPropertyMoniker Name="ExternalPublicPort/ComponentPortMoniker" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isInGac">
            <DomainPropertyMoniker Name="ExternalPublicPort/IsInGac" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ExternalComponentShape" MonikerAttributeName="" MonikerElementName="externalComponentShapeMoniker" ElementName="externalComponentShape" MonikerTypeName="ExternalComponentShapeMoniker">
        <GeometryShapeMoniker Name="ExternalComponentShape" />
      </XmlClassData>
      <XmlClassData TypeName="ExternalPublicPortShape" MonikerAttributeName="" MonikerElementName="externalPublicPortShapeMoniker" ElementName="externalPublicPortShape" MonikerTypeName="ExternalPublicPortShapeMoniker">
        <PortMoniker Name="ExternalPublicPortShape" />
      </XmlClassData>
      <XmlClassData TypeName="AssociationLink" MonikerAttributeName="" MonikerElementName="associationLinkMoniker" ElementName="associationLink" MonikerTypeName="AssociationLinkMoniker">
        <ConnectorMoniker Name="AssociationLink" />
      </XmlClassData>
      <XmlClassData TypeName="DataLayer" MonikerAttributeName="" SerializeId="true" MonikerElementName="modelsLayerMoniker" ElementName="modelsLayer" MonikerTypeName="DataLayerMoniker">
        <DomainClassMoniker Name="DataLayer" />
        <ElementData>
          <XmlRelationshipData RoleElementName="packages">
            <DomainRelationshipMoniker Name="DataLayerHasPackages" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="xmlNamespace">
            <DomainPropertyMoniker Name="DataLayer/XmlNamespace" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="referencedExternalComponents">
            <DomainRelationshipMoniker Name="DataLayerReferencesExternalComponent" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Enumeration" MonikerAttributeName="" SerializeId="true" MonikerElementName="enumerationMoniker" ElementName="enumeration" MonikerTypeName="EnumerationMoniker">
        <DomainClassMoniker Name="Enumeration" />
        <ElementData>
          <XmlPropertyData XmlName="isFlag">
            <DomainPropertyMoniker Name="Enumeration/IsFlag" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="values">
            <DomainRelationshipMoniker Name="EnumHasValues" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityShape" MonikerAttributeName="" MonikerElementName="entityShapeMoniker" ElementName="entityShape" MonikerTypeName="EntityShapeMoniker">
        <CompartmentShapeMoniker Name="EntityShape" />
      </XmlClassData>
      <XmlClassData TypeName="EnumTypeShape" MonikerAttributeName="" MonikerElementName="enumTypeShapeMoniker" ElementName="enumTypeShape" MonikerTypeName="EnumTypeShapeMoniker">
        <CompartmentShapeMoniker Name="EnumTypeShape" />
      </XmlClassData>
      <XmlClassData TypeName="UIWorkflowLayer" MonikerAttributeName="" SerializeId="true" MonikerElementName="uiWorkflowLayerMoniker" ElementName="uiWorkflowLayer" MonikerTypeName="UIWorkflowLayerMoniker">
        <DomainClassMoniker Name="UIWorkflowLayer" />
        <ElementData>
          <XmlRelationshipData RoleElementName="scenarios">
            <DomainRelationshipMoniker Name="AppWorkflowLayerHasScenarios" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Scenario" MonikerAttributeName="" SerializeId="true" MonikerElementName="scenarioMoniker" ElementName="scenario" MonikerTypeName="ScenarioMoniker">
        <DomainClassMoniker Name="Scenario" />
        <ElementData>
          <XmlRelationshipData RoleElementName="views">
            <DomainRelationshipMoniker Name="ScenarioHasUIView" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="contracts">
            <DomainRelationshipMoniker Name="ScenarioUsesContracts" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="UIView" MonikerAttributeName="" SerializeId="true" MonikerElementName="uiViewMoniker" ElementName="uiView" MonikerTypeName="UIViewMoniker">
        <DomainClassMoniker Name="UIView" />
        <ElementData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="viewTargets">
            <DomainRelationshipMoniker Name="Action" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="UIView/Description" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ScenarioHasUIView" MonikerAttributeName="" MonikerElementName="scenarioHasUIViewMoniker" ElementName="scenarioHasUIView" MonikerTypeName="ScenarioHasUIViewMoniker">
        <DomainRelationshipMoniker Name="ScenarioHasUIView" />
      </XmlClassData>
      <XmlClassData TypeName="AppWorkflowLayerHasScenarios" MonikerAttributeName="" MonikerElementName="appWorkflowLayerHasScenariosMoniker" ElementName="appWorkflowLayerHasScenarios" MonikerTypeName="AppWorkflowLayerHasScenariosMoniker">
        <DomainRelationshipMoniker Name="AppWorkflowLayerHasScenarios" />
      </XmlClassData>
      <XmlClassData TypeName="ScenarioShape" MonikerAttributeName="" MonikerElementName="scenarioShapeMoniker" ElementName="scenarioShape" MonikerTypeName="ScenarioShapeMoniker">
        <GeometryShapeMoniker Name="ScenarioShape" />
      </XmlClassData>
      <XmlClassData TypeName="UiViewShape" MonikerAttributeName="" MonikerElementName="uiViewShapeMoniker" ElementName="uiViewShape" MonikerTypeName="UiViewShapeMoniker">
        <GeometryShapeMoniker Name="UiViewShape" />
      </XmlClassData>
      <XmlClassData TypeName="Action" MonikerAttributeName="" SerializeId="true" MonikerElementName="actionMoniker" ElementName="action" MonikerTypeName="ActionMoniker">
        <DomainRelationshipMoniker Name="Action" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="Action/Name" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="dependencyProperties">
            <DomainRelationshipMoniker Name="ActionHasDependencyProperties" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="roles">
            <DomainPropertyMoniker Name="Action/Roles" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="Action/Description" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ActionLink" MonikerAttributeName="" MonikerElementName="actionLinkMoniker" ElementName="actionLink" MonikerTypeName="ActionLinkMoniker">
        <ConnectorMoniker Name="ActionLink" />
      </XmlClassData>
      <XmlClassData TypeName="UIWorkflowLayerShape" MonikerAttributeName="" MonikerElementName="uIWorkflowLayerShapeMoniker" ElementName="uIWorkflowLayerShape" MonikerTypeName="UIWorkflowLayerShapeMoniker">
        <GeometryShapeMoniker Name="UIWorkflowLayerShape" />
      </XmlClassData>
      <XmlClassData TypeName="DotNetAssembly" MonikerAttributeName="" SerializeId="true" MonikerElementName="dotNetAssemblyMoniker" ElementName="dotNetAssembly" MonikerTypeName="DotNetAssemblyMoniker">
        <DomainClassMoniker Name="DotNetAssembly" />
        <ElementData>
          <XmlPropertyData XmlName="fullName">
            <DomainPropertyMoniker Name="DotNetAssembly/FullName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isInGac">
            <DomainPropertyMoniker Name="DotNetAssembly/IsInGac" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="version">
            <DomainPropertyMoniker Name="DotNetAssembly/Version" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="initialLocation">
            <DomainPropertyMoniker Name="DotNetAssembly/InitialLocation" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="internalAssemblyReferences">
            <DomainRelationshipMoniker Name="AssemblyReferencesAssemblies" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="visibility">
            <DomainPropertyMoniker Name="DotNetAssembly/Visibility" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DotnetAssemblyShape" MonikerAttributeName="" MonikerElementName="dotnetAssemblyShapeMoniker" ElementName="dotnetAssemblyShape" MonikerTypeName="DotnetAssemblyShapeMoniker">
        <GeometryShapeMoniker Name="DotnetAssemblyShape" />
      </XmlClassData>
      <XmlClassData TypeName="AbstractLayer" MonikerAttributeName="" SerializeId="true" MonikerElementName="abstractLayerMoniker" ElementName="abstractLayer" MonikerTypeName="AbstractLayerMoniker">
        <DomainClassMoniker Name="AbstractLayer" />
        <ElementData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="externalServiceReferences">
            <DomainRelationshipMoniker Name="ExternalServiceReference" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="artifacts">
            <DomainRelationshipMoniker Name="LayerHasArtifacts" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="assemblyName">
            <DomainPropertyMoniker Name="AbstractLayer/AssemblyName" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="configurations">
            <DomainRelationshipMoniker Name="LayerHasConfigurationParts" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Entity" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityMoniker" ElementName="entity" MonikerTypeName="EntityMoniker">
        <DomainClassMoniker Name="Entity" />
        <ElementData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="targets">
            <DomainRelationshipMoniker Name="Association" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="baseType">
            <DomainPropertyMoniker Name="Entity/BaseType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isAbstract">
            <DomainPropertyMoniker Name="Entity/IsAbstract" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="properties">
            <DomainRelationshipMoniker Name="EntityHasProperties" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="customAttributes">
            <DomainPropertyMoniker Name="Entity/CustomAttributes" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="tableName">
            <DomainPropertyMoniker Name="Entity/TableName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="tableOwner">
            <DomainPropertyMoniker Name="Entity/TableOwner" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="databaseType">
            <DomainPropertyMoniker Name="Entity/DatabaseType" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="entityHasSubClasses">
            <DomainRelationshipMoniker Name="EntityHasSubClasses" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="subClasses">
            <DomainRelationshipMoniker Name="Generalization" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EnumValue" MonikerAttributeName="" SerializeId="true" MonikerElementName="enumValueMoniker" ElementName="enumValue" MonikerTypeName="EnumValueMoniker">
        <DomainClassMoniker Name="EnumValue" />
        <ElementData>
          <XmlPropertyData XmlName="value">
            <DomainPropertyMoniker Name="EnumValue/Value" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="hasValue">
            <DomainPropertyMoniker Name="EnumValue/HasValue" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EnumHasValues" MonikerAttributeName="" MonikerElementName="enumHasValuesMoniker" ElementName="enumHasValues" MonikerTypeName="EnumHasValuesMoniker">
        <DomainRelationshipMoniker Name="EnumHasValues" />
      </XmlClassData>
      <XmlClassData TypeName="Artifact" MonikerAttributeName="" SerializeId="true" MonikerElementName="artifactMoniker" ElementName="artifact" MonikerTypeName="ArtifactMoniker">
        <DomainClassMoniker Name="Artifact" />
        <ElementData>
          <XmlPropertyData XmlName="fileName">
            <DomainPropertyMoniker Name="Artifact/FileName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="type">
            <DomainPropertyMoniker Name="Artifact/Type" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="initialFileName">
            <DomainPropertyMoniker Name="Artifact/InitialFileName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="scope">
            <DomainPropertyMoniker Name="Artifact/Scope" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="configurationMode">
            <DomainPropertyMoniker Name="Artifact/ConfigurationMode" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="BinaryComponent" MonikerAttributeName="" SerializeId="true" MonikerElementName="binaryComponentMoniker" ElementName="binaryComponent" MonikerTypeName="BinaryComponentMoniker">
        <DomainClassMoniker Name="BinaryComponent" />
        <ElementData>
          <XmlRelationshipData RoleElementName="assemblies">
            <DomainRelationshipMoniker Name="BinaryComponentHasAssemblies" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="BinaryComponentShape" MonikerAttributeName="" MonikerElementName="binaryComponentShapeMoniker" ElementName="binaryComponentShape" MonikerTypeName="BinaryComponentShapeMoniker">
        <GeometryShapeMoniker Name="BinaryComponentShape" />
      </XmlClassData>
      <XmlClassData TypeName="EntityHasProperties" MonikerAttributeName="" MonikerElementName="entityHasPropertiesMoniker" ElementName="entityHasProperties" MonikerTypeName="EntityHasPropertiesMoniker">
        <DomainRelationshipMoniker Name="EntityHasProperties" />
      </XmlClassData>
      <XmlClassData TypeName="ElementHasDependencyProperties" MonikerAttributeName="" MonikerElementName="elementHasDependencyPropertiesMoniker" ElementName="elementHasDependencyProperties" MonikerTypeName="ElementHasDependencyPropertiesMoniker">
        <DomainRelationshipMoniker Name="ElementHasDependencyProperties" />
      </XmlClassData>
      <XmlClassData TypeName="AssemblyReferencesAssemblies" MonikerAttributeName="" MonikerElementName="assemblyReferencesAssembliesMoniker" ElementName="assemblyReferencesAssemblies" MonikerTypeName="AssemblyReferencesAssembliesMoniker">
        <DomainRelationshipMoniker Name="AssemblyReferencesAssemblies" />
        <ElementData>
          <XmlPropertyData XmlName="scope">
            <DomainPropertyMoniker Name="AssemblyReferencesAssemblies/Scope" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="AssemblyReferencesAssemblyLink" MonikerAttributeName="" MonikerElementName="assemblyReferencesAssemblyLinkMoniker" ElementName="assemblyReferencesAssemblyLink" MonikerTypeName="AssemblyReferencesAssemblyLinkMoniker">
        <ConnectorMoniker Name="AssemblyReferencesAssemblyLink" />
      </XmlClassData>
      <XmlClassData TypeName="ImplementationLink" MonikerAttributeName="" MonikerElementName="implementationLinkMoniker" ElementName="implementationLink" MonikerTypeName="ImplementationLinkMoniker">
        <ConnectorMoniker Name="ImplementationLink" />
      </XmlClassData>
      <XmlClassData TypeName="LayerPackage" MonikerAttributeName="" SerializeId="true" MonikerElementName="layerPackageMoniker" ElementName="layerPackage" MonikerTypeName="LayerPackageMoniker">
        <DomainClassMoniker Name="LayerPackage" />
        <ElementData>
          <XmlPropertyData XmlName="level">
            <DomainPropertyMoniker Name="LayerPackage/Level" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="layers">
            <DomainRelationshipMoniker Name="LayerPackageContainsLayers" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="interfaceLayer">
            <DomainRelationshipMoniker Name="LayerPackageReferencesInterfaceLayer" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="LayerPackageShape" MonikerAttributeName="" MonikerElementName="layerPackageShapeMoniker" ElementName="layerPackageShape" MonikerTypeName="LayerPackageShapeMoniker">
        <GeometryShapeMoniker Name="LayerPackageShape" />
      </XmlClassData>
      <XmlClassData TypeName="ConfigurationPart" MonikerAttributeName="" MonikerElementName="configurationPartMoniker" ElementName="configurationPart" MonikerTypeName="ConfigurationPartMoniker">
        <DomainClassMoniker Name="ConfigurationPart" />
        <ElementData>
          <XmlPropertyData XmlName="xmlContent" Representation="Element">
            <DomainPropertyMoniker Name="ConfigurationPart/XmlContent" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="ConfigurationPart/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="enabled">
            <DomainPropertyMoniker Name="ConfigurationPart/Enabled" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="visibility">
            <DomainPropertyMoniker Name="ConfigurationPart/Visibility" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="AssociationHasProperties" MonikerAttributeName="" MonikerElementName="associationHasPropertiesMoniker" ElementName="associationHasProperties" MonikerTypeName="AssociationHasPropertiesMoniker">
        <DomainRelationshipMoniker Name="AssociationHasProperties" />
      </XmlClassData>
      <XmlClassData TypeName="ActionHasDependencyProperties" MonikerAttributeName="" MonikerElementName="actionHasDependencyPropertiesMoniker" ElementName="actionHasDependencyProperties" MonikerTypeName="ActionHasDependencyPropertiesMoniker">
        <DomainRelationshipMoniker Name="ActionHasDependencyProperties" />
      </XmlClassData>
      <XmlClassData TypeName="ExternalServiceReferenceLink" MonikerAttributeName="" MonikerElementName="externalServiceReferenceLinkMoniker" ElementName="externalServiceReferenceLink" MonikerTypeName="ExternalServiceReferenceLinkMoniker">
        <ConnectorMoniker Name="ExternalServiceReferenceLink" />
      </XmlClassData>
      <XmlClassData TypeName="AssociationHasForeignKeys" MonikerAttributeName="" MonikerElementName="associationHasForeignKeysMoniker" ElementName="associationHasForeignKeys" MonikerTypeName="AssociationHasForeignKeysMoniker">
        <DomainRelationshipMoniker Name="AssociationHasForeignKeys" />
      </XmlClassData>
      <XmlClassData TypeName="ForeignKey" MonikerAttributeName="" MonikerElementName="foreignKeyMoniker" ElementName="foreignKey" MonikerTypeName="ForeignKeyMoniker">
        <DomainClassMoniker Name="ForeignKey" />
        <ElementData>
          <XmlRelationshipData RoleElementName="column">
            <DomainRelationshipMoniker Name="ForeignKeyReferencesProperty" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="primaryKey">
            <DomainRelationshipMoniker Name="ForeignKeyReferencesPrimaryKey" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ForeignKeyReferencesProperty" MonikerAttributeName="" MonikerElementName="foreignKeyReferencesPropertyMoniker" ElementName="foreignKeyReferencesProperty" MonikerTypeName="ForeignKeyReferencesPropertyMoniker">
        <DomainRelationshipMoniker Name="ForeignKeyReferencesProperty" />
      </XmlClassData>
      <XmlClassData TypeName="ForeignKeyReferencesPrimaryKey" MonikerAttributeName="" MonikerElementName="foreignKeyReferencesPrimaryKeyMoniker" ElementName="foreignKeyReferencesPrimaryKey" MonikerTypeName="ForeignKeyReferencesPrimaryKeyMoniker">
        <DomainRelationshipMoniker Name="ForeignKeyReferencesPrimaryKey" />
      </XmlClassData>
      <XmlClassData TypeName="TypeWithOperations" MonikerAttributeName="" SerializeId="true" MonikerElementName="typeWithOperationsMoniker" ElementName="typeWithOperations" MonikerTypeName="TypeWithOperationsMoniker">
        <DomainClassMoniker Name="TypeWithOperations" />
        <ElementData>
          <XmlRelationshipData RoleElementName="operations">
            <DomainRelationshipMoniker Name="TypeWithOperationsHasOperations" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="servicesUsed">
            <DomainRelationshipMoniker Name="ClassUsesOperations" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="customAttributes">
            <DomainPropertyMoniker Name="TypeWithOperations/CustomAttributes" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="TypeWithOperationsHasOperations" MonikerAttributeName="" MonikerElementName="typeWithOperationsHasOperationsMoniker" ElementName="typeWithOperationsHasOperations" MonikerTypeName="TypeWithOperationsHasOperationsMoniker">
        <DomainRelationshipMoniker Name="TypeWithOperationsHasOperations" />
      </XmlClassData>
      <XmlClassData TypeName="ServiceContract" MonikerAttributeName="" SerializeId="true" MonikerElementName="serviceContractMoniker" ElementName="serviceContract" MonikerTypeName="ServiceContractMoniker">
        <DomainClassMoniker Name="ServiceContract" />
      </XmlClassData>
      <XmlClassData TypeName="InterfaceLayer" MonikerAttributeName="" SerializeId="true" MonikerElementName="interfaceLayerMoniker" ElementName="interfaceLayer" MonikerTypeName="InterfaceLayerMoniker">
        <DomainClassMoniker Name="InterfaceLayer" />
        <ElementData>
          <XmlRelationshipData RoleElementName="serviceContracts">
            <DomainRelationshipMoniker Name="InterfaceLayerHasContracts" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="level">
            <DomainPropertyMoniker Name="InterfaceLayer/Level" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ModelRootHasComponent" MonikerAttributeName="" MonikerElementName="modelRootHasComponentMoniker" ElementName="modelRootHasComponent" MonikerTypeName="ModelRootHasComponentMoniker">
        <DomainRelationshipMoniker Name="ModelRootHasComponent" />
      </XmlClassData>
      <XmlClassData TypeName="Component" MonikerAttributeName="" SerializeId="true" MonikerElementName="componentMoniker" ElementName="component" MonikerTypeName="ComponentMoniker">
        <DomainClassMoniker Name="Component" />
        <ElementData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="Component/Namespace" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ExternalServiceReference" MonikerAttributeName="" MonikerElementName="externalServiceReferenceMoniker" ElementName="externalServiceReference" MonikerTypeName="ExternalServiceReferenceMoniker">
        <DomainRelationshipMoniker Name="ExternalServiceReference" />
        <ElementData>
          <XmlPropertyData XmlName="configurationMode">
            <DomainPropertyMoniker Name="ExternalServiceReference/ConfigurationMode" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="scope">
            <DomainPropertyMoniker Name="ExternalServiceReference/Scope" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="dependencyProperties">
            <DomainRelationshipMoniker Name="ExternalServiceReferenceHasDependencyProperties" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Layer" MonikerAttributeName="" SerializeId="true" MonikerElementName="layerMoniker" ElementName="layer" MonikerTypeName="LayerMoniker">
        <DomainClassMoniker Name="Layer" />
        <ElementData>
          <XmlPropertyData XmlName="hostingContext">
            <DomainPropertyMoniker Name="Layer/HostingContext" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="classes">
            <DomainRelationshipMoniker Name="LayerHasClassImplementations" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="startupProject">
            <DomainPropertyMoniker Name="Layer/StartupProject" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="LayerHasArtifacts" MonikerAttributeName="" MonikerElementName="layerHasArtifactsMoniker" ElementName="layerHasArtifacts" MonikerTypeName="LayerHasArtifactsMoniker">
        <DomainRelationshipMoniker Name="LayerHasArtifacts" />
      </XmlClassData>
      <XmlClassData TypeName="InterfaceLayerHasContracts" MonikerAttributeName="" MonikerElementName="interfaceLayerHasContractsMoniker" ElementName="interfaceLayerHasContracts" MonikerTypeName="InterfaceLayerHasContractsMoniker">
        <DomainRelationshipMoniker Name="InterfaceLayerHasContracts" />
      </XmlClassData>
      <XmlClassData TypeName="SoftwareLayer" MonikerAttributeName="" SerializeId="true" MonikerElementName="softwareLayerMoniker" ElementName="softwareLayer" MonikerTypeName="SoftwareLayerMoniker">
        <DomainClassMoniker Name="SoftwareLayer" />
        <ElementData>
          <XmlPropertyData XmlName="template">
            <DomainPropertyMoniker Name="SoftwareLayer/Template" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="SoftwareLayer/Namespace" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="vSProjectName">
            <DomainPropertyMoniker Name="SoftwareLayer/VSProjectName" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ExternalServiceReferenceHasDependencyProperties" MonikerAttributeName="" MonikerElementName="externalServiceReferenceHasDependencyPropertiesMoniker" ElementName="externalServiceReferenceHasDependencyProperties" MonikerTypeName="ExternalServiceReferenceHasDependencyPropertiesMoniker">
        <DomainRelationshipMoniker Name="ExternalServiceReferenceHasDependencyProperties" />
      </XmlClassData>
      <XmlClassData TypeName="InterfaceLayerShape" MonikerAttributeName="" MonikerElementName="interfaceLayerShapeMoniker" ElementName="interfaceLayerShape" MonikerTypeName="InterfaceLayerShapeMoniker">
        <GeometryShapeMoniker Name="InterfaceLayerShape" />
      </XmlClassData>
      <XmlClassData TypeName="LayerPackageContainsLayers" MonikerAttributeName="" MonikerElementName="layerPackageContainsLayersMoniker" ElementName="layerPackageContainsLayers" MonikerTypeName="LayerPackageContainsLayersMoniker">
        <DomainRelationshipMoniker Name="LayerPackageContainsLayers" />
      </XmlClassData>
      <XmlClassData TypeName="SoftwareComponentHasLayers" MonikerAttributeName="" MonikerElementName="softwareComponentHasLayersMoniker" ElementName="softwareComponentHasLayers" MonikerTypeName="SoftwareComponentHasLayersMoniker">
        <DomainRelationshipMoniker Name="SoftwareComponentHasLayers" />
      </XmlClassData>
      <XmlClassData TypeName="BinaryComponentHasAssemblies" MonikerAttributeName="" MonikerElementName="binaryComponentHasAssembliesMoniker" ElementName="binaryComponentHasAssemblies" MonikerTypeName="BinaryComponentHasAssembliesMoniker">
        <DomainRelationshipMoniker Name="BinaryComponentHasAssemblies" />
      </XmlClassData>
      <XmlClassData TypeName="ComponentHasLayerPackages" MonikerAttributeName="" MonikerElementName="componentHasLayerPackagesMoniker" ElementName="componentHasLayerPackages" MonikerTypeName="ComponentHasLayerPackagesMoniker">
        <DomainRelationshipMoniker Name="ComponentHasLayerPackages" />
      </XmlClassData>
      <XmlClassData TypeName="ServiceContractShape" MonikerAttributeName="" MonikerElementName="serviceContractShapeMoniker" ElementName="serviceContractShape" MonikerTypeName="ServiceContractShapeMoniker">
        <CompartmentShapeMoniker Name="ServiceContractShape" />
      </XmlClassData>
      <XmlClassData TypeName="ClassImplementation" MonikerAttributeName="" SerializeId="true" MonikerElementName="classImplementationMoniker" ElementName="classImplementation" MonikerTypeName="ClassImplementationMoniker">
        <DomainClassMoniker Name="ClassImplementation" />
        <ElementData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="contract">
            <DomainRelationshipMoniker Name="Implementation" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="associatedEntity">
            <DomainRelationshipMoniker Name="ClassImplementationReferencesAssociatedEntity" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="LayerHasClassImplementations" MonikerAttributeName="" MonikerElementName="layerHasClassImplementationsMoniker" ElementName="layerHasClassImplementations" MonikerTypeName="LayerHasClassImplementationsMoniker">
        <DomainRelationshipMoniker Name="LayerHasClassImplementations" />
      </XmlClassData>
      <XmlClassData TypeName="Implementation" MonikerAttributeName="" MonikerElementName="implementationMoniker" ElementName="implementation" MonikerTypeName="ImplementationMoniker">
        <DomainRelationshipMoniker Name="Implementation" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="Implementation/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="configurationMode">
            <DomainPropertyMoniker Name="Implementation/ConfigurationMode" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="dependencyProperties">
            <DomainRelationshipMoniker Name="ImplementationHasDependencyProperties" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ClassImplementationReferencesAssociatedEntity" MonikerAttributeName="" MonikerElementName="classImplementationReferencesAssociatedEntityMoniker" ElementName="classImplementationReferencesAssociatedEntity" MonikerTypeName="ClassImplementationReferencesAssociatedEntityMoniker">
        <DomainRelationshipMoniker Name="ClassImplementationReferencesAssociatedEntity" />
      </XmlClassData>
      <XmlClassData TypeName="ClassUsesOperationLink" MonikerAttributeName="" MonikerElementName="classUsesOperationLinkMoniker" ElementName="classUsesOperationLink" MonikerTypeName="ClassUsesOperationLinkMoniker">
        <ConnectorMoniker Name="ClassUsesOperationLink" />
      </XmlClassData>
      <XmlClassData TypeName="ClassImplementationShape" MonikerAttributeName="" MonikerElementName="classImplementationShapeMoniker" ElementName="classImplementationShape" MonikerTypeName="ClassImplementationShapeMoniker">
        <CompartmentShapeMoniker Name="ClassImplementationShape" />
      </XmlClassData>
      <XmlClassData TypeName="ImplementationHasDependencyProperties" MonikerAttributeName="" MonikerElementName="implementationHasDependencyPropertiesMoniker" ElementName="implementationHasDependencyProperties" MonikerTypeName="ImplementationHasDependencyPropertiesMoniker">
        <DomainRelationshipMoniker Name="ImplementationHasDependencyProperties" />
      </XmlClassData>
      <XmlClassData TypeName="LayerPackageReferencesInterfaceLayer" MonikerAttributeName="" MonikerElementName="layerPackageReferencesInterfaceLayerMoniker" ElementName="layerPackageReferencesInterfaceLayer" MonikerTypeName="LayerPackageReferencesInterfaceLayerMoniker">
        <DomainRelationshipMoniker Name="LayerPackageReferencesInterfaceLayer" />
      </XmlClassData>
      <XmlClassData TypeName="LayerHasConfigurationParts" MonikerAttributeName="" MonikerElementName="layerHasConfigurationPartsMoniker" ElementName="layerHasConfigurationParts" MonikerTypeName="LayerHasConfigurationPartsMoniker">
        <DomainRelationshipMoniker Name="LayerHasConfigurationParts" />
      </XmlClassData>
      <XmlClassData TypeName="Process" MonikerAttributeName="" SerializeId="true" MonikerElementName="processMoniker" ElementName="process" MonikerTypeName="ProcessMoniker">
        <DomainClassMoniker Name="Process" />
      </XmlClassData>
      <XmlClassData TypeName="ProcessShape" MonikerAttributeName="" MonikerElementName="processShapeMoniker" ElementName="processShape" MonikerTypeName="ProcessShapeMoniker">
        <GeometryShapeMoniker Name="ProcessShape" />
      </XmlClassData>
      <XmlClassData TypeName="ExternalServiceContract" MonikerAttributeName="" SerializeId="true" MonikerElementName="externalServiceContractMoniker" ElementName="externalServiceContract" MonikerTypeName="ExternalServiceContractMoniker">
        <DomainClassMoniker Name="ExternalServiceContract" />
      </XmlClassData>
      <XmlClassData TypeName="ExternalServiceContractShape" MonikerAttributeName="" MonikerElementName="externalServiceContractShapeMoniker" ElementName="externalServiceContractShape" MonikerTypeName="ExternalServiceContractShapeMoniker">
        <PortMoniker Name="ExternalServiceContractShape" />
      </XmlClassData>
      <XmlClassData TypeName="ExternalComponentHasPublicPorts" MonikerAttributeName="" MonikerElementName="externalComponentHasPublicPortsMoniker" ElementName="externalComponentHasPublicPorts" MonikerTypeName="ExternalComponentHasPublicPortsMoniker">
        <DomainRelationshipMoniker Name="ExternalComponentHasPublicPorts" />
      </XmlClassData>
      <XmlClassData TypeName="ScenarioThumbnailShape" MonikerAttributeName="" MonikerElementName="scenarioThumbnailShapeMoniker" ElementName="scenarioThumbnailShape" MonikerTypeName="ScenarioThumbnailShapeMoniker">
        <GeometryShapeMoniker Name="ScenarioThumbnailShape" />
      </XmlClassData>
      <XmlClassData TypeName="ScenarioUsesContracts" MonikerAttributeName="" MonikerElementName="scenarioUsesContractsMoniker" ElementName="scenarioUsesContracts" MonikerTypeName="ScenarioUsesContractsMoniker">
        <DomainRelationshipMoniker Name="ScenarioUsesContracts" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="ScenarioUsesContracts/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="configurationMode">
            <DomainPropertyMoniker Name="ScenarioUsesContracts/ConfigurationMode" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ScenarioUsesContractsLink" MonikerAttributeName="" MonikerElementName="scenarioUsesContractsLinkMoniker" ElementName="scenarioUsesContractsLink" MonikerTypeName="ScenarioUsesContractsLinkMoniker">
        <ConnectorMoniker Name="ScenarioUsesContractsLink" />
      </XmlClassData>
      <XmlClassData TypeName="ClassUsesOperations" MonikerAttributeName="" MonikerElementName="classUsesOperationsMoniker" ElementName="classUsesOperations" MonikerTypeName="ClassUsesOperationsMoniker">
        <DomainRelationshipMoniker Name="ClassUsesOperations" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="ClassUsesOperations/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="configurationMode">
            <DomainPropertyMoniker Name="ClassUsesOperations/ConfigurationMode" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="singleton">
            <DomainPropertyMoniker Name="ClassUsesOperations/Singleton" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="scope">
            <DomainPropertyMoniker Name="ClassUsesOperations/Scope" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="dependencyProperties">
            <DomainRelationshipMoniker Name="ClassUsesOperationsHasDependencyProperties" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ClassUsesOperationsHasDependencyProperties" MonikerAttributeName="" MonikerElementName="classUsesOperationsHasDependencyPropertiesMoniker" ElementName="classUsesOperationsHasDependencyProperties" MonikerTypeName="ClassUsesOperationsHasDependencyPropertiesMoniker">
        <DomainRelationshipMoniker Name="ClassUsesOperationsHasDependencyProperties" />
      </XmlClassData>
      <XmlClassData TypeName="DataLayerReferencesExternalComponent" MonikerAttributeName="" MonikerElementName="modelsLayerReferencesExternalComponentMoniker" ElementName="modelsLayerReferencesExternalComponent" MonikerTypeName="DataLayerReferencesExternalComponentMoniker">
        <DomainRelationshipMoniker Name="DataLayerReferencesExternalComponent" />
        <ElementData>
          <XmlPropertyData XmlName="configurationMode">
            <DomainPropertyMoniker Name="DataLayerReferencesExternalComponent/ConfigurationMode" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="scope">
            <DomainPropertyMoniker Name="DataLayerReferencesExternalComponent/Scope" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DataLayerReferencesExternalComponentLink" MonikerAttributeName="" MonikerElementName="modelsLayerReferencesExternalComponentLinkMoniker" ElementName="modelsLayerReferencesExternalComponentLink" MonikerTypeName="DataLayerReferencesExternalComponentLinkMoniker">
        <ConnectorMoniker Name="DataLayerReferencesExternalComponentLink" />
      </XmlClassData>
      <XmlClassData TypeName="EntityHasSubClasses" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasSubClassesMoniker" ElementName="entityHasSubClasses" MonikerTypeName="EntityHasSubClassesMoniker">
        <DomainRelationshipMoniker Name="EntityHasSubClasses" />
      </XmlClassData>
      <XmlClassData TypeName="GeneralizationLink" MonikerAttributeName="" MonikerElementName="generalizationLinkMoniker" ElementName="generalizationLink" MonikerTypeName="GeneralizationLinkMoniker">
        <ConnectorMoniker Name="GeneralizationLink" />
      </XmlClassData>
      <XmlClassData TypeName="Generalization" MonikerAttributeName="" MonikerElementName="generalizationMoniker" ElementName="generalization" MonikerTypeName="GeneralizationMoniker">
        <DomainRelationshipMoniker Name="Generalization" />
      </XmlClassData>
    </ClassData>
  </XmlSerializationBehavior>
  <ExplorerBehavior Name="SystemModelExplorer">
    <CustomNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\database.bmp" ShowsDomainClass="true">
        <Class>
          <DomainClassMoniker Name="DataAccessLayer" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\VSObject_Library.bmp" ShowsDomainClass="true">
        <Class>
          <DomainClassMoniker Name="DataLayer" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\gif\Composants.gif">
        <Class>
          <DomainClassMoniker Name="SoftwareComponent" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\gear_1.bmp" ShowsDomainClass="true">
        <Class>
          <DomainClassMoniker Name="BusinessLayer" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\VSObject_Interface.bmp" ShowsDomainClass="true">
        <Class>
          <DomainClassMoniker Name="InterfaceLayer" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\doc.png">
        <Class>
          <DomainClassMoniker Name="Entity" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\VSObject_Class.bmp">
        <Class>
          <DomainClassMoniker Name="ClassImplementation" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\VSObject_Interface.bmp">
        <Class>
          <DomainClassMoniker Name="ServiceContract" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\VSObject_Enum.bmp" ShowsDomainClass="true">
        <Class>
          <DomainClassMoniker Name="Enumeration" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\VSObject_Namespace.bmp">
        <Class>
          <DomainClassMoniker Name="Package" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\appworkflowshapeicon.bmp" ShowsDomainClass="true">
        <Class>
          <DomainRelationshipMoniker Name="SoftwareComponentHasLayers" />
        </Class>
      </ExplorerNodeSettings>
    </CustomNodeSettings>
    <HiddenNodes>
      <DomainPath>ComponentHasLayerPackages.LayerPackages</DomainPath>
      <DomainPath>ExternalServiceReferenceHasDependencyProperties.DependencyProperties</DomainPath>
      <DomainPath>ElementHasDependencyProperties.DependencyProperties</DomainPath>
      <DomainPath>ImplementationHasDependencyProperties.DependencyProperties</DomainPath>
    </HiddenNodes>
  </ExplorerBehavior>
  <ConnectionBuilders>
    <ConnectionBuilder Name="AssociationBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="Association" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Entity" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Entity" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="ActionBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="Action" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="UIView" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="UIView" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="EnumHasValuesBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="EnumHasValues" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Enumeration" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="EnumValue" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="ReferenceBuilder" IsCustom="true" />
    <ConnectionBuilder Name="LayerPackageContainsLayersBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="LayerPackageContainsLayers" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="LayerPackage" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true">
            <AcceptingClass>
              <DomainClassMoniker Name="Layer" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="ClassImplementationReferencesAssociatedEntityBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="ClassImplementationReferencesAssociatedEntity" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="ClassImplementation" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Entity" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="GeneralizationBuilder" IsCustom="true">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="EntityHasSubClasses" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Entity" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Entity" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
  </ConnectionBuilders>
  <Diagram Id="4aaab11a-70bc-416d-a87f-01da4a8d8817" Description="Base diagram for ComponentModelDiagram" Name="ComponentModelDiagram" DisplayName="ComponentModelDiagram" Namespace="DSLFactory.Candle.SystemModel" GeneratesDoubleDerived="true">
    <Class>
      <DomainClassMoniker Name="CandleModel" />
    </Class>
    <ShapeMaps>
      <ShapeMap HasCustomParentElement="true">
        <DomainClassMoniker Name="DataAccessLayer" />
        <DecoratorMap>
          <TextDecoratorMoniker Name="DataAccessLayerShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="DataAccessLayerShape" />
      </ShapeMap>
      <ShapeMap HasCustomParentElement="true">
        <DomainClassMoniker Name="PresentationLayer" />
        <DecoratorMap>
          <TextDecoratorMoniker Name="PresentationLayerShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="PresentationLayerShape" />
      </ShapeMap>
      <ShapeMap HasCustomParentElement="true">
        <DomainClassMoniker Name="DataLayer" />
        <DecoratorMap>
          <TextDecoratorMoniker Name="DataLayerShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="DataLayerShape" />
      </ShapeMap>
      <ShapeMap HasCustomParentElement="true">
        <DomainClassMoniker Name="Package" />
        <ParentElementPath>
          <DomainPath>DataLayerHasPackages.Layer/!Layer</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="PackageShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="PackageShape" />
      </ShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="Enumeration" />
        <ParentElementPath>
          <DomainPath>PackageHasTypes.Package/!Package</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="EnumTypeShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="EnumTypeShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="EnumTypeShape/Values" />
          <ElementsDisplayed>
            <DomainPath>EnumHasValues.Values/!Value</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="Entity" />
        <ParentElementPath>
          <DomainPath>PackageHasTypes.Package/!Package</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="EntityShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="EntityShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="EntityShape/Properties" />
          <ElementsDisplayed>
            <DomainPath>EntityHasProperties.Properties/!Property</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="TypeMember/DisplayName" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="ExternalPublicPort" />
        <ParentElementPath>
          <DomainPath>ExternalComponentHasPublicPorts.Parent/!Parent</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="ExternalPublicPortShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <PortMoniker Name="ExternalPublicPortShape" />
      </ShapeMap>
      <ShapeMap HasCustomParentElement="true">
        <DomainClassMoniker Name="BusinessLayer" />
        <DecoratorMap>
          <TextDecoratorMoniker Name="BusinessLayerShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="BusinessLayerShape" />
      </ShapeMap>
      <ShapeMap HasCustomParentElement="true">
        <DomainClassMoniker Name="UIView" />
        <ParentElementPath>
          <DomainPath>ScenarioHasUIView.Scenario/!Scenario</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="UiViewShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="UiViewShape" />
      </ShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="Scenario" />
        <ParentElementPath>
          <DomainPath>AppWorkflowLayerHasScenarios.Layer/!Layer</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="ScenarioShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="ScenarioShape" />
      </ShapeMap>
      <ShapeMap HasCustomParentElement="true">
        <DomainClassMoniker Name="UIWorkflowLayer" />
        <DecoratorMap>
          <TextDecoratorMoniker Name="UIWorkflowLayerShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="UIWorkflowLayerShape" />
      </ShapeMap>
      <ShapeMap HasCustomParentElement="true">
        <DomainClassMoniker Name="DotNetAssembly" />
        <DecoratorMap>
          <TextDecoratorMoniker Name="DotnetAssemblyShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <IconDecoratorMoniker Name="DotnetAssemblyShape/IconDecorator1" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="DotNetAssembly/Visibility" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="Public" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <GeometryShapeMoniker Name="DotnetAssemblyShape" />
      </ShapeMap>
      <ShapeMap HasCustomParentElement="true">
        <DomainClassMoniker Name="LayerPackage" />
        <ParentElementPath>
          <DomainPath />
        </ParentElementPath>
        <GeometryShapeMoniker Name="LayerPackageShape" />
      </ShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="SoftwareComponent" />
        <ParentElementPath>
          <DomainPath>ModelRootHasComponent.Model/!CandleModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="SoftwareComponentShape/HeaderText" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="SoftwareComponentShape" />
      </ShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="BinaryComponent" />
        <ParentElementPath>
          <DomainPath>ModelRootHasComponent.Model/!CandleModel</DomainPath>
        </ParentElementPath>
        <GeometryShapeMoniker Name="BinaryComponentShape" />
      </ShapeMap>
      <ShapeMap HasCustomParentElement="true">
        <DomainClassMoniker Name="InterfaceLayer" />
        <ParentElementPath>
          <DomainPath />
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="InterfaceLayerShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="InterfaceLayerShape" />
      </ShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="ServiceContract" />
        <ParentElementPath>
          <DomainPath>InterfaceLayerHasContracts.Layer/!InterfaceLayer</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="ServiceContractShape/TextDecorator1" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="ServiceContractShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="ServiceContractShape/Operations" />
          <ElementsDisplayed>
            <DomainPath>TypeWithOperationsHasOperations.Operations/!Operation</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="TypeMember/DisplayName" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="ClassImplementation" />
        <ParentElementPath>
          <DomainPath>LayerHasClassImplementations.Layer/!Layer</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="ClassImplementationShape/TextDecorator1" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="ClassImplementationShape" />
      </CompartmentShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="ExternalServiceContract" />
        <ParentElementPath>
          <DomainPath>ExternalComponentHasPublicPorts.Parent/!Parent</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="ExternalServiceContractShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <PortMoniker Name="ExternalServiceContractShape" />
      </ShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="Process" />
        <ParentElementPath>
          <DomainPath>LayerHasClassImplementations.Layer/!Layer/SoftwareComponentHasLayers.Component/!SoftwareComponent/ModelRootHasComponent.Model/!CandleModel</DomainPath>
        </ParentElementPath>
        <GeometryShapeMoniker Name="ProcessShape" />
      </ShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="ExternalComponent" />
        <ParentElementPath>
          <DomainPath>CandleModelHasExternalComponents.Model/!CandleModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <IconDecoratorMoniker Name="ExternalComponentShape/IsLastVersionDecorator" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="ExternalComponent/IsLastVersion" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="False" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="ExternalComponentShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="ExternalComponentShape/VersionDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ExternalComponent/Version" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="ExternalComponentShape" />
      </ShapeMap>
    </ShapeMaps>
    <ConnectorMaps>
      <ConnectorMap>
        <ConnectorMoniker Name="AssociationLink" />
        <DomainRelationshipMoniker Name="Association" />
        <DecoratorMap>
          <TextDecoratorMoniker Name="AssociationLink/SourceDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Association/SourceRoleName" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="AssociationLink/TargetDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Association/TargetRoleName" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="AssociationLink/SourceMultiplicityDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Association/SourceMultiplicity" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="AssociationLink/TargetMultiplicityDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Association/TargetMultiplicity" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="ActionLink" />
        <DomainRelationshipMoniker Name="Action" />
        <DecoratorMap>
          <TextDecoratorMoniker Name="ActionLink/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Action/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="AssemblyReferencesAssemblyLink" />
        <DomainRelationshipMoniker Name="AssemblyReferencesAssemblies" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="ImplementationLink" />
        <DomainRelationshipMoniker Name="Implementation" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="ExternalServiceReferenceLink" />
        <DomainRelationshipMoniker Name="ExternalServiceReference" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="ScenarioUsesContractsLink" />
        <DomainRelationshipMoniker Name="ScenarioUsesContracts" />
      </ConnectorMap>
      <ConnectorMap ConnectsCustomTarget="true">
        <ConnectorMoniker Name="ClassUsesOperationLink" />
        <DomainRelationshipMoniker Name="ClassUsesOperations" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="DataLayerReferencesExternalComponentLink" />
        <DomainRelationshipMoniker Name="DataLayerReferencesExternalComponent" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="GeneralizationLink" />
        <DomainRelationshipMoniker Name="EntityHasSubClasses" />
      </ConnectorMap>
    </ConnectorMaps>
  </Diagram>
  <Designer FileExtension="cml" EditorGuid="a347c751-7722-4fa1-b73e-2e03db41d1c9">
    <RootClass>
      <DomainClassMoniker Name="CandleModel" />
    </RootClass>
    <XmlSerializationDefinition CustomPostLoad="false">
      <XmlSerializationBehaviorMoniker Name="CandleSerializationBehavior" />
    </XmlSerializationDefinition>
    <ToolboxTab TabText="SystemModel">
      <ElementTool Name="Implementation" ToolboxIcon="Resources\portshapeicon.bmp" Caption="Implementation Class" Tooltip="Implementation" HelpKeyword="ImplementationHelp">
        <DomainClassMoniker Name="ClassImplementation" />
      </ElementTool>
      <ConnectionTool Name="Reference" ToolboxIcon="Resources\ExampleConnectorToolBitmap.bmp" Caption="Reference" Tooltip="Reference" HelpKeyword="ReferenceHelp">
        <ConnectionBuilderMoniker Name="Candle/ReferenceBuilder" />
      </ConnectionTool>
      <ElementTool Name="Contract" ToolboxIcon="Resources\VSObject_Interface.bmp" Caption="Contract" Tooltip="Contract" HelpKeyword="Contract">
        <DomainClassMoniker Name="ServiceContract" />
      </ElementTool>
      <ElementTool Name="Process" ToolboxIcon="Resources\appworkflowshapeicon.bmp" Caption="Process" Tooltip="Process" HelpKeyword="Process">
        <DomainClassMoniker Name="Process" />
      </ElementTool>
    </ToolboxTab>
    <ToolboxTab TabText="Models">
      <ElementTool Name="PackageModel" ToolboxIcon="Resources\VSObject_Namespace.bmp" Caption="Namespace" Tooltip="Package Model" HelpKeyword="PackageModelHelp">
        <DomainClassMoniker Name="Package" />
      </ElementTool>
      <ElementTool Name="EntityModel" ToolboxIcon="Resources\VSObject_Class.bmp" Caption="Entity" Tooltip="Defined Type" HelpKeyword="DefinedTypeHelp">
        <DomainClassMoniker Name="Entity" />
      </ElementTool>
      <ElementTool Name="EnumModel" ToolboxIcon="Resources\VSObject_Enum.bmp" Caption="Enum" Tooltip="Enum Model" HelpKeyword="EnumModelHelp">
        <DomainClassMoniker Name="Enumeration" />
      </ElementTool>
      <ConnectionTool Name="Association" ToolboxIcon="Resources\UnidirectionTool.bmp" Caption="Association" Tooltip="Association" HelpKeyword="AssociationHelp">
        <ConnectionBuilderMoniker Name="Candle/AssociationBuilder" />
      </ConnectionTool>
      <ConnectionTool Name="Generalization" ToolboxIcon="Resources\GeneralizationTool.bmp" Caption="Generalization" Tooltip="Generalization" HelpKeyword="Generalization">
        <ConnectionBuilderMoniker Name="Candle/GeneralizationBuilder" />
      </ConnectionTool>
    </ToolboxTab>
    <ToolboxTab TabText="UILayer">
      <ElementTool Name="Scenario" ToolboxIcon="Resources\ApplicationShapeIcon.bmp" Caption="Scenario" Tooltip="Scenario" HelpKeyword="ScenarioHelp">
        <DomainClassMoniker Name="Scenario" />
      </ElementTool>
      <ElementTool Name="UiView" ToolboxIcon="Resources\modelclassicon.bmp" Caption="View" Tooltip="Ui View" HelpKeyword="UIViewId">
        <DomainClassMoniker Name="UIView" />
      </ElementTool>
      <ConnectionTool Name="Action" ToolboxIcon="Resources\ExampleConnectorToolBitmap.bmp" Caption="Action" Tooltip="Action" HelpKeyword="ActionHelp">
        <ConnectionBuilderMoniker Name="Candle/ActionBuilder" />
      </ConnectionTool>
    </ToolboxTab>
    <ToolboxTab TabText="Layers">
      <ElementTool Name="Component" ToolboxIcon="Resources\Composants.bmp" Caption="Software Component" Tooltip="Component" HelpKeyword="ComponentHelpKeyword">
        <DomainClassMoniker Name="SoftwareComponent" />
      </ElementTool>
      <ElementTool Name="ExternalSoftwareComponent" ToolboxIcon="Resources\ApplicationShapeIcon.bmp" Caption="External Component" Tooltip="External Component" HelpKeyword="ExternalComponentHelp">
        <DomainClassMoniker Name="ExternalComponent" />
      </ElementTool>
      <ElementTool Name="ExternalAssembly" ToolboxIcon="Resources\ApplicationShapeIcon.bmp" Caption="Assembly" Tooltip="External Assembly" HelpKeyword="ExternalAssemblyHelp">
        <DomainClassMoniker Name="DotNetAssembly" />
      </ElementTool>
      <ElementTool Name="PresentationLayer" ToolboxIcon="Resources\webuilayershapeicon.bmp" Caption="Generic Presentation Layer" Tooltip="Presentation Layer" HelpKeyword="PresentationLayerHelp">
        <DomainClassMoniker Name="PresentationLayer" />
      </ElementTool>
      <ElementTool Name="UIWorkflowLayer" ToolboxIcon="Resources\appworkflowshapeicon.bmp" Caption="UI Process" Tooltip="UIWorkflow Layer" HelpKeyword="UIWorkflowLayer">
        <DomainClassMoniker Name="UIWorkflowLayer" />
      </ElementTool>
      <ElementTool Name="BusinessLayer" ToolboxIcon="Resources\ServicesLayerShapeIcon.bmp" Caption="Business Layer" Tooltip="Business Layer" HelpKeyword="BusinessLayerHelp">
        <DomainClassMoniker Name="BusinessLayer" />
      </ElementTool>
      <ElementTool Name="DataAccessLayer" ToolboxIcon="Resources\database.bmp" Caption="Data Access Layer" Tooltip="Data Access Layer" HelpKeyword="DALLayerHelp">
        <DomainClassMoniker Name="DataAccessLayer" />
      </ElementTool>
      <ElementTool Name="DataLayerModel" ToolboxIcon="Resources\VSObject_Library.bmp" Caption="DataLayer" Tooltip="Data Layer Model" HelpKeyword="DataLayerModelHelp">
        <DomainClassMoniker Name="DataLayer" />
      </ElementTool>
      <ElementTool Name="InterfaceLayer" ToolboxIcon="Resources\VSObject_Interface.bmp" Caption="Interfaces Layer" Tooltip="Interface Layer" HelpKeyword="InterfaceLayer">
        <DomainClassMoniker Name="InterfaceLayer" />
      </ElementTool>
      <ElementTool Name="Library" ToolboxIcon="Resources\ExampleShapeToolBitmap.bmp" Caption="Library" Tooltip="Library" HelpKeyword="Library">
        <DomainClassMoniker Name="BinaryComponent" />
      </ElementTool>
    </ToolboxTab>
    <Validation UsesMenu="true" UsesOpen="true" UsesSave="true" UsesCustom="true" UsesLoad="false" />
    <DiagramMoniker Name="ComponentModelDiagram" />
  </Designer>
  <Explorer ExplorerGuid="f6628fb0-4d5d-4071-a697-13afaa5a686b" Title="Component Explorer">
    <ExplorerBehaviorMoniker Name="Candle/SystemModelExplorer" />
  </Explorer>
</Dsl>