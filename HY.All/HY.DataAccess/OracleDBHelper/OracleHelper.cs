using System;
using System.Configuration;
using System.Data;
using System.Collections;
//using Oracle.DataAccess.Client;
using System.Data.OracleClient;
using System.Data.Common;


namespace HY.DataAccess.OracleDBHelper
{

    public class OracleHelper
    {
        #region ˽�з����͹���
        //sql
        private static void PrepareCommand(OracleCommand command, OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters, out bool mustCloseConnection)
        {
            
            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }


        //ͨ��
        private static void PrepareCommand(OracleCommand command, DbConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, DbParameter[] commandParameters, out bool mustCloseConnection)
        {

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;

                connection.Open();


            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = (OracleConnection)connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;
            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }



        //sql
        private static void AttachParameters(OracleCommand command, OracleParameter[] commandParameters)
        {

            if (commandParameters != null)
            {
                foreach (OracleParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        //ͨ��
        private static void AttachParameters(OracleCommand command, DbParameter[] commandParameters)
        {

            if (commandParameters != null)
            {
                foreach (DbParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        #endregion

        #region transaction ������




        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <param name="conn">���ݿ�����</param>
        /// <param name="Iso">ָ�����ӵ�����������Ϊ</param>
        /// <returns>��ǰ����</returns>  
        public static DbTransaction BeginTransaction(OracleConnection conn, IsolationLevel Iso)
        {
            conn.Open();
            return conn.BeginTransaction(Iso);
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <param name="conn">���ݿ�����</param>
        /// <returns>��ǰ����</returns>
        public static DbTransaction BeginTransaction(OracleConnection conn)
        {

            conn.Open();
            return conn.BeginTransaction();
        }

        /// <summary>
        /// ��������ȷ�ϲ���
        /// </summary>
        /// <param name="Transaction">Ҫ����������</param>
        public static void endTransactionCommit(DbTransaction Transaction)
        {
            DbConnection con = (DbConnection)Transaction.Connection;
            Transaction.Commit();
            con.Close();
        }

        /// <summary>
        /// �������񣬻ع�����
        /// </summary>
        /// <param name="Transaction">Ҫ����������</param>
        public static void endTransactionRollback(DbTransaction Transaction)
        {
            DbConnection con = (DbConnection)Transaction.Connection;
            Transaction.Rollback();
            con.Close();
        }

        #endregion



        #region ExecuteNonQuery




        /// <summary>
        ///  ִ�Уӣѣ������ߴ洢���� ,�����ز���,ֻ����Ӱ������(ͨ��)
        /// </summary>
        /// <param name="transaction">������ڵ�����</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="commandParameters">�ӣѣ������ߴ洢���̲���</param>
        /// <returns>Ӱ�������</returns>
        public static int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            //Ҫ������  
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, ((OracleTransaction)transaction).Connection, (OracleTransaction)transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            int retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return retval;
        }





        /// <summary>
        ///  ִ�Уӣѣ������ߴ洢���� ,�����ز���,ֻ����Ӱ������
        /// </summary>
        /// <param name="transaction">������ڵ�����</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <returns>Ӱ�������</returns>
        public static int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(transaction, commandType, commandText, (DbParameter[])null);
        }


        /// <summary>
        ///  ִ�Уӣѣ������ߴ洢���� ,�����ز���,ֻ����Ӱ������(ͨ��)
        /// </summary>
        /// <param name="connection">Ҫִ�Уӣѣ���������</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="commandParameters">�ӣѣ������ߴ洢���̲���</param>
        /// <returns>Ӱ�������</returns>
        public static int ExecuteNonQuery(OracleConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            int retval = 0;
            //Ҫ������
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);


            retval = cmd.ExecuteNonQuery();


            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }



        /// <summary>
        ///  ִ�Уӣѣ������ߴ洢���� ,�����ز���,ֻ����Ӱ������
        /// </summary>
        /// <param name="connection">Ҫִ�Уӣѣ���������</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <returns>Ӱ�������</returns>
        public static int ExecuteNonQuery(OracleConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(connection, commandType, commandText, (DbParameter[])null);
        }

        #endregion

        #region ExecuteDataset


        /// <summary>
        /// ִ�Уӣѣ������ߴ洢���� ,���ز���dataset(ͨ��)
        /// </summary>
        /// <param name="connection">Ҫִ�Уӣѣ���������</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="Table"> ���ı��� </param>
        /// <param name="commandParameters">�ӣѣ������ߴ洢���̲���</param>
        /// <returns>ִ�н����</returns>
        public static DataSet ExecuteDataset(OracleConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {

            OracleCommand cmd = new OracleCommand();
            //cmd.InitialLONGFetchSize = -1;
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);
            using (OracleDataAdapter da = new OracleDataAdapter(cmd))
            {
                DataSet ds = new DataSet();


                da.Fill(ds);


                cmd.Parameters.Clear();
                if (mustCloseConnection)
                    connection.Close();
                return ds;
            }
        }


        /// <summary>
        /// ִ�Уӣѣ������ߴ洢���� ,���ز���dataset
        /// </summary>
        /// <param name="connection">Ҫִ�Уӣѣ���������</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="Table">���ı���</param>
        /// <returns>ִ�н����</returns>��
        public static DataSet ExecuteDataset(OracleConnection connection, CommandType commandType, string commandText)
        {

            return ExecuteDataset(connection, commandType, commandText, (DbParameter[])null);
        }


        #endregion

        #region ExecuteReader



        //ͨ��
        private static OracleDataReader ExecuteReader(OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, DbParameter[] commandParameters, bool isClose)
        {
            bool mustCloseConnection = false;

            OracleCommand cmd = new OracleCommand();

            PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            OracleDataReader dataReader = null;


            if (isClose)
            {
                dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            else
            {
                dataReader = cmd.ExecuteReader();
            }




            bool canClear = true;
            foreach (DbParameter commandParameter in cmd.Parameters)
            {
                if (commandParameter.Direction != ParameterDirection.Input)
                    canClear = false;
            }

            if (canClear)
            {
                cmd.Parameters.Clear();
            }

            return dataReader;

        }



        /// <summary>
        /// ִ�Уӣѣ������ߴ洢���� ,���ز���datareader(ͨ��)
        /// <remarks >
        /// ��Ҫ��ʾ�ر�����
        /// </remarks>
        /// </summary>
        /// <param name="connection">Ҫִ�Уӣѣ���������</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="commandParameters">�ӣѣ������ߴ洢���̲���</param>
        /// <returns>ִ�н����</returns>
        public static OracleDataReader ExecuteReader(OracleConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {

            return ExecuteReader(connection, (OracleTransaction)null, commandType, commandText, commandParameters, true);
        }


        /// <summary>
        /// ִ�Уӣѣ������ߴ洢���� ,���ز���datareader
        /// <remarks >
        /// ��Ҫ��ʾ�ر�����
        /// </remarks>
        /// </summary>
        /// <param name="connection">Ҫִ�Уӣѣ���������</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>n
        /// <returns>ִ�н����</returns>
        public static OracleDataReader ExecuteReader(OracleConnection connection, CommandType commandType, string commandText)
        {

            return ExecuteReader(connection, commandType, commandText, (DbParameter[])null);
        }


        #endregion

        #region ExecuteScalar


        /// <summary>
        /// ִ�Уӣѣ������ߴ洢���� ,���ز���object����һ�У���һ�е�ֵ(ͨ��)
        /// </summary>
        /// <param name="connection">Ҫִ�Уӣѣ���������</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="commandParameters">�ӣѣ������ߴ洢���̲���</param>
        /// <returns>ִ�н������һ�У���һ�е�ֵ</returns>��
        public static object ExecuteScalar(OracleConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            object retval = null;
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            retval = cmd.ExecuteScalar();


            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }

        /// <summary>
        /// ִ�Уӣѣ������ߴ洢���� ,���ز���object����һ�У���һ�е�ֵ
        /// </summary>
        /// <param name="connection">Ҫִ�Уӣѣ���������</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <returns>ִ�н������һ�У���һ�е�ֵ</returns>��
        public static object ExecuteScalar(OracleConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connection, commandType, commandText, (DbParameter[])null);
        }




        /// <summary>
        ///  ִ�Уӣѣ������ߴ洢���� ,���ز���object����һ�У���һ�е�ֵ
        /// </summary>
        /// <param name="transaction">������ڵ�����</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="commandParameters">�ӣѣ������ߴ洢���̲���</param>
        /// <returns>Ӱ�������</returns>
        public static object ExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText)
        {
            object retval = null;
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, ((OracleTransaction)transaction).Connection, (OracleTransaction)transaction, commandType, commandText, (DbParameter[])null, out mustCloseConnection);
            retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return retval;
        }


        /// <summary>
        ///  ִ�Уӣѣ������ߴ洢���� ,���ز���object����һ�У���һ�е�ֵ
        /// </summary>
        /// <param name="transaction">������ڵ�����</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="commandParameters">�ӣѣ������ߴ洢���̲���</param>
        /// <returns>Ӱ�������</returns>
        public static object ExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            object retval = null;
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, ((OracleTransaction)transaction).Connection, (OracleTransaction)transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return retval;
        }

        #endregion


    }
}
