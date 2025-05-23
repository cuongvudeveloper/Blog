import React from "react";

function Logo(): React.JSX.Element {
  return (
    <div className="flex items-center justify-center gap-1 font-medium">
      <div className="w-7 h-7 rounded-full flex items-center justify-center bg-black dark:bg-white text-white dark:text-black text-xl">
        H
      </div>
      <div>Hanover</div>
    </div>
  );
}

export default Logo;
