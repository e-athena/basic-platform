import { Navigate, Outlet } from '@umijs/max';

const App: React.FC = (props) => {
  // const { isLogin } = useAuth();
  console.log(props);
  if (true) {
    return <Outlet />;
  } else {
    return <Navigate to="/login" />;
  }
};
export default App;
