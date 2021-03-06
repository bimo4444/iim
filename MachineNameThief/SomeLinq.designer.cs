﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MachineNameThief
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="GalAMM_test")]
	public partial class SomeLinqDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertSecuritySystemUser(SecuritySystemUser instance);
    partial void UpdateSecuritySystemUser(SecuritySystemUser instance);
    partial void DeleteSecuritySystemUser(SecuritySystemUser instance);
    #endregion
		
		public SomeLinqDataContext() : 
				base(global::MachineNameThief.Properties.Settings.Default.GalAMM_testConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public SomeLinqDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SomeLinqDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SomeLinqDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SomeLinqDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<SecuritySystemUser> SecuritySystemUsers
		{
			get
			{
				return this.GetTable<SecuritySystemUser>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SecuritySystemUser")]
	public partial class SecuritySystemUser : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _Oid;
		
		private string _StoredPassword;
		
		private System.Nullable<bool> _ChangePasswordOnFirstLogon;
		
		private string _UserName;
		
		private System.Nullable<bool> _IsActive;
		
		private System.Nullable<int> _OptimisticLockField;
		
		private System.Nullable<int> _GCRecord;
		
		private System.Nullable<int> _ObjectType;
		
		private string _ФИО;
		
		private string _Примечание;
		
		private string _Компьютер;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnOidChanging(System.Guid value);
    partial void OnOidChanged();
    partial void OnStoredPasswordChanging(string value);
    partial void OnStoredPasswordChanged();
    partial void OnChangePasswordOnFirstLogonChanging(System.Nullable<bool> value);
    partial void OnChangePasswordOnFirstLogonChanged();
    partial void OnUserNameChanging(string value);
    partial void OnUserNameChanged();
    partial void OnIsActiveChanging(System.Nullable<bool> value);
    partial void OnIsActiveChanged();
    partial void OnOptimisticLockFieldChanging(System.Nullable<int> value);
    partial void OnOptimisticLockFieldChanged();
    partial void OnGCRecordChanging(System.Nullable<int> value);
    partial void OnGCRecordChanged();
    partial void OnObjectTypeChanging(System.Nullable<int> value);
    partial void OnObjectTypeChanged();
    partial void OnФИОChanging(string value);
    partial void OnФИОChanged();
    partial void OnПримечаниеChanging(string value);
    partial void OnПримечаниеChanged();
    partial void OnКомпьютерChanging(string value);
    partial void OnКомпьютерChanged();
    #endregion
		
		public SecuritySystemUser()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Oid", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid Oid
		{
			get
			{
				return this._Oid;
			}
			set
			{
				if ((this._Oid != value))
				{
					this.OnOidChanging(value);
					this.SendPropertyChanging();
					this._Oid = value;
					this.SendPropertyChanged("Oid");
					this.OnOidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StoredPassword", DbType="NVarChar(MAX)")]
		public string StoredPassword
		{
			get
			{
				return this._StoredPassword;
			}
			set
			{
				if ((this._StoredPassword != value))
				{
					this.OnStoredPasswordChanging(value);
					this.SendPropertyChanging();
					this._StoredPassword = value;
					this.SendPropertyChanged("StoredPassword");
					this.OnStoredPasswordChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ChangePasswordOnFirstLogon", DbType="Bit")]
		public System.Nullable<bool> ChangePasswordOnFirstLogon
		{
			get
			{
				return this._ChangePasswordOnFirstLogon;
			}
			set
			{
				if ((this._ChangePasswordOnFirstLogon != value))
				{
					this.OnChangePasswordOnFirstLogonChanging(value);
					this.SendPropertyChanging();
					this._ChangePasswordOnFirstLogon = value;
					this.SendPropertyChanged("ChangePasswordOnFirstLogon");
					this.OnChangePasswordOnFirstLogonChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="NVarChar(100)")]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this.OnUserNameChanging(value);
					this.SendPropertyChanging();
					this._UserName = value;
					this.SendPropertyChanged("UserName");
					this.OnUserNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsActive", DbType="Bit")]
		public System.Nullable<bool> IsActive
		{
			get
			{
				return this._IsActive;
			}
			set
			{
				if ((this._IsActive != value))
				{
					this.OnIsActiveChanging(value);
					this.SendPropertyChanging();
					this._IsActive = value;
					this.SendPropertyChanged("IsActive");
					this.OnIsActiveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OptimisticLockField", DbType="Int")]
		public System.Nullable<int> OptimisticLockField
		{
			get
			{
				return this._OptimisticLockField;
			}
			set
			{
				if ((this._OptimisticLockField != value))
				{
					this.OnOptimisticLockFieldChanging(value);
					this.SendPropertyChanging();
					this._OptimisticLockField = value;
					this.SendPropertyChanged("OptimisticLockField");
					this.OnOptimisticLockFieldChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GCRecord", DbType="Int")]
		public System.Nullable<int> GCRecord
		{
			get
			{
				return this._GCRecord;
			}
			set
			{
				if ((this._GCRecord != value))
				{
					this.OnGCRecordChanging(value);
					this.SendPropertyChanging();
					this._GCRecord = value;
					this.SendPropertyChanged("GCRecord");
					this.OnGCRecordChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ObjectType", DbType="Int")]
		public System.Nullable<int> ObjectType
		{
			get
			{
				return this._ObjectType;
			}
			set
			{
				if ((this._ObjectType != value))
				{
					this.OnObjectTypeChanging(value);
					this.SendPropertyChanging();
					this._ObjectType = value;
					this.SendPropertyChanged("ObjectType");
					this.OnObjectTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ФИО", DbType="NVarChar(100)")]
		public string ФИО
		{
			get
			{
				return this._ФИО;
			}
			set
			{
				if ((this._ФИО != value))
				{
					this.OnФИОChanging(value);
					this.SendPropertyChanging();
					this._ФИО = value;
					this.SendPropertyChanged("ФИО");
					this.OnФИОChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Примечание", DbType="NVarChar(100)")]
		public string Примечание
		{
			get
			{
				return this._Примечание;
			}
			set
			{
				if ((this._Примечание != value))
				{
					this.OnПримечаниеChanging(value);
					this.SendPropertyChanging();
					this._Примечание = value;
					this.SendPropertyChanged("Примечание");
					this.OnПримечаниеChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Компьютер", DbType="NVarChar(100)")]
		public string Компьютер
		{
			get
			{
				return this._Компьютер;
			}
			set
			{
				if ((this._Компьютер != value))
				{
					this.OnКомпьютерChanging(value);
					this.SendPropertyChanging();
					this._Компьютер = value;
					this.SendPropertyChanged("Компьютер");
					this.OnКомпьютерChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
