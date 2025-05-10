import PropTypes from "prop-types";
import React from "react";

function Input({ className, ...props }) {
  return (
    <input
      className={`w-full text-sm p-2 h-9 shadow-md bg-gray-50 rounded-sm focus:bg-white ${className}`}
      {...props}
    />
  );
}

Input.propTypes = {
  className: PropTypes.string,
};

export default Input;
