import { Route, Routes } from "react-router-dom";

import SideMenu from "./components/SideMenu/index";
import ClassDocDisplay from "./components/ClassDocDisplay/index";
import FunctionDocDisplay from "./components/FunctionDocDisplay/index";
import Overview from "./components/Overview";
import CodeCity from "./components/CodeCity";

import documentation from "../output.json";

const App = () => {
  return (
    <div
      style={{
        display: "flex",
        height: "100vh",
        flexDirection: "column",
        overflow: "auto",
      }}
    >
      <div style={{ display: "flex", flex: 1 }}>
        <SideMenu documentation={documentation} />
        <div style={{ flex: 1, padding: "20px" }}>
          <Routes>
            <Route
              path="/"
              element={<Overview overview={documentation.overview} />}
            />
            <Route
              path="/class/:className"
              element={<ClassDocDisplay documentation={documentation} />}
            />
            <Route
              path="/function/:functionName"
              element={<FunctionDocDisplay documentation={documentation} />}
            />
            <Route
              path="/codecity"
              element={<CodeCity documentation={documentation} />}
            />
          </Routes>
        </div>
      </div>
    </div>
  );
};

export default App;
